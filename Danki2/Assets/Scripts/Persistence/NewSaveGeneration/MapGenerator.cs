using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    private readonly Dictionary<int, Zone> depthToZoneLookup = new Dictionary<int, Zone>();
    private readonly Dictionary<Zone, int> zoneIntroductionDepthsLookup = new Dictionary<Zone, int>();
    private readonly Dictionary<Zone, int> bossDepthsLookup = new Dictionary<Zone, int>();
    private int finalNodeDepth;
    
    protected override bool DestroyOnLoad => false;

    private void Start()
    {
        int zoneIntroductionDepth = 1;
        EnumUtils.ForEach<Zone>(zone =>
        {
            zoneIntroductionDepthsLookup[zone] = zoneIntroductionDepth;
            int newDepth = zoneIntroductionDepth + MapGenerationLookup.Instance.RoomsPerZoneLookup[zone];
            for (int i = zoneIntroductionDepth; i < newDepth; i++) depthToZoneLookup[i] = zone;
            zoneIntroductionDepth = newDepth;
            bossDepthsLookup[zone] = zoneIntroductionDepth - 1;
        });

        finalNodeDepth = zoneIntroductionDepth - 1;
    }

    public RoomNode Generate()
    {
        RoomNode rootNode = new RoomNode {Depth = 1};

        rootNode.IterateDown(GenerateChildren, node => node.Depth <= MapGenerationLookup.Instance.GeneratedRoomDepth);
        rootNode.IterateDown(SetZone);
        rootNode.IterateDown(SetDepthInZone);
        rootNode.IterateDown(SetRoomType);
        SetRoomData(rootNode, Pole.South);
        rootNode.IterateDown(SetRoomData, node => !node.IsRootNode && !node.IsLeafNode);
        rootNode.IterateDown(SetIndicatorData);

        return rootNode;
    }

    public void GenerateNextLayer(RoomNode currentRoomNode)
    {
        if (currentRoomNode.HasVictoryRoom()) return;

        List<RoomNode> leafNodes = new List<RoomNode>();
        currentRoomNode.IterateDown(
            node => leafNodes.Add(node),
            node => node.IsLeafNode
        );

        leafNodes.ForEach(TryGenerateVictoryNode);
        bool hasVictoryRoom = currentRoomNode.HasVictoryRoom();

        if (!hasVictoryRoom)
        {
            leafNodes.ForEach(GenerateChildren);
            leafNodes.ForEach(node => node.Children.ForEach(SetZone));
            leafNodes.ForEach(node => node.Children.ForEach(SetDepthInZone));
            leafNodes.ForEach(node => node.Children.ForEach(SetRoomType));
        }
        
        leafNodes.ForEach(SetRoomData);
        
        if (!hasVictoryRoom) leafNodes.ForEach(node => node.Children.ForEach(SetIndicatorData));
    }

    private void GenerateChildren(RoomNode node)
    {
        int numberOfChildren = Random.Range(
            MapGenerationLookup.Instance.MinRoomExits,
            MapGenerationLookup.Instance.MaxRoomExits + 1
        );

        Utils.Repeat(numberOfChildren, () => node.Children.Add(new RoomNode
        {
            Parent = node,
            Depth = node.Depth + 1
        }));
    }

    private void SetZone(RoomNode node) => node.Zone = depthToZoneLookup[node.Depth];

    private void SetDepthInZone(RoomNode node) => node.DepthInZone = node.Depth - zoneIntroductionDepthsLookup[node.Zone] + 1;
    
    /// <summary>
    /// Sets the room type according to the following strategy:
    ///  - If the room is at the required depth, then set its room type to either zone introduction or boss accordingly.
    ///  - For each room type, find the distance from the node to the nearest parent in the same zone with that room type,
    ///  - If no such parent exists, assume that the introduction node to the zone was of that room type. That is to
    ///    assume that the introduction nodes are special nodes of every room type,
    ///  - If there is no weight available for a room type (i.e. it's weight array does not have a value for that
    ///    room type's distance), then mark that room type as required,
    ///  - If there are any required room types, then select one randomly,
    ///  - Otherwise, select a room randomly from all room types weighted according to the relevant weight for each
    ///    room type, which depends on the distance from the node and a parent with that room type.
    /// </summary>
    private void SetRoomType(RoomNode node)
    {
        if (zoneIntroductionDepthsLookup[node.Zone] == node.Depth)
        {
            node.RoomType = RoomType.ZoneIntroduction;
            return;
        }
        
        if (bossDepthsLookup[node.Zone] == node.Depth)
        {
            node.RoomType = RoomType.Boss;
            return;
        }
        
        Dictionary<RoomType, int> distancesFromPreviousRoomTypes = new Dictionary<RoomType, int>();
        MapGenerationLookup.Instance.ForEachRoomTypeInPool(roomType =>
        {
            int distance = node.GetDistanceFromPreviousRoomTypes(roomType, RoomType.ZoneIntroduction);
            distancesFromPreviousRoomTypes[roomType] = distance;
        });

        List<RoomType> requiredRoomTypes = new List<RoomType>();
        MapGenerationLookup.Instance.ForEachRoomTypeInPool(roomType =>
        {
            if (distancesFromPreviousRoomTypes[roomType] >= MapGenerationLookup.Instance.GetDistanceWhenRequired(roomType))
            {
                requiredRoomTypes.Add(roomType);
            }
        });

        if (requiredRoomTypes.Count != 0)
        {
            node.RoomType = RandomUtils.Choice(requiredRoomTypes);
            return;
        }

        List<RoomType> choices = new List<RoomType>();
        MapGenerationLookup.Instance.ForEachRoomTypeInPool(roomType =>
        {
            int weighting = MapGenerationLookup.Instance.GetWeight(roomType, distancesFromPreviousRoomTypes[roomType]);
            Utils.Repeat(weighting, () => choices.Add(roomType));
        });

        node.RoomType = RandomUtils.Choice(choices);
    }

    private void SetRoomData(RoomNode node)
    {
        Pole trueParentExitDirection = SceneLookup.Instance.GetTrueExitDirection(
            node.Parent.Scene,
            node.Parent.CameraOrientation,
            node.Parent.ChildToExitIdLookup[node]
        );
        SetRoomData(node, OrientationUtils.GetReversedPole(trueParentExitDirection));
    }
    
    private void SetRoomData(RoomNode node, Pole trueEntranceDirection)
    {
        SetCommonData(node, trueEntranceDirection);
        
        switch (node.RoomType)
        {
            case RoomType.Combat:
                SetCombatData(node);
                break;
            case RoomType.Boss:
                SetBossData(node);
                break;
            case RoomType.Ability:
                SetAbilityData(node);
                break;
        }
    }
    
    private void SetCommonData(RoomNode node, Pole trueEntranceDirection)
    {
        node.Scene = RandomUtils.Choice(SceneLookup.Instance.GetValidScenes(
            node.Zone,
            node.RoomType,
            trueEntranceDirection,
            node.Children.Count
        ));
        node.CameraOrientation = RandomUtils.Choice(SceneLookup.Instance.GetValidCameraOrientations(
            node.Scene,
            trueEntranceDirection,
            node.Children.Count
        ));
        node.PlayerSpawnerId = RandomUtils.Choice(SceneLookup.Instance.GetValidEntranceIds(
            node.Scene,
            trueEntranceDirection,
            node.CameraOrientation
        ));

        node.ModuleSeed = RandomUtils.Seed();
        node.TransitionModuleSeed = RandomUtils.Seed();

        SetTransitionData(node);
    }
    
    private void SetTransitionData(RoomNode node)
    {
        List<int> validExitIds = SceneLookup.Instance.GetValidExitIds(
            node.Scene,
            node.CameraOrientation,
            node.PlayerSpawnerId
        );

        node.Children.ForEach(child =>
        {
            int exitId = RandomUtils.Choice(validExitIds);
            node.ExitIdToChildLookup[exitId] = child;
            node.ChildToExitIdLookup[child] = exitId;
            node.ExitIdToIndicatesNextRoomType[exitId] = false;
            node.ExitIdToFurtherIndicatedRoomTypes[exitId] = new List<RoomType>();
            validExitIds.Remove(exitId);
        });
    }

    private void SetCombatData(RoomNode node)
    {
        List<ActorType> spawnedEnemies = MapGenerationLookup.Instance.SpawnedEnemiesPerDepthLookup[node.Depth].SpawnedEnemies;
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            node.CombatRoomSaveData.SpawnerIdToSpawnedActor[i] = spawnedEnemies[i];
        }
    }

    private void SetBossData(RoomNode node)
    {
        node.CombatRoomSaveData.SpawnerIdToSpawnedActor[0] = ActorType.Bear;
    }

    private void SetAbilityData(RoomNode node)
    {
        List<AbilityReference> choices = new List<AbilityReference>();
        EnumUtils.ForEach<AbilityReference>(abilityReference =>
        {
            if (!AbilityLookup.Instance.PlayerCanCast(abilityReference)) return;

            Rarity rarity = AbilityLookup.Instance.GetRarity(abilityReference);
            int weighting = RarityLookup.Instance.Lookup[rarity].Weighting;
            Utils.Repeat(weighting, () => choices.Add(abilityReference));
        });

        Utils.Repeat(MapGenerationLookup.Instance.AbilityChoices, () =>
        {
            AbilityReference choice = RandomUtils.Choice(choices);
            node.AbilityRoomSaveData.AbilityChoices.Add(choice);
            choices.RemoveAll(c => c == choice);
        });
    }

    private void SetIndicatorData(RoomNode node)
    {
        if (node.IsRootNode) return;
        if (node.RoomType == RoomType.ZoneIntroduction) return;

        RoomNode parent = node.Parent;

        bool isIndicatedInParent =
            parent.RoomType != RoomType.Boss &&
            (node.RoomType == RoomType.Boss ||
            Random.value <= MapGenerationLookup.Instance.ChanceIndicatesChildRoomType);
        if (!isIndicatedInParent) return;

        parent.ExitIdToIndicatesNextRoomType[parent.ChildToExitIdLookup[node]] = true;

        if (parent.IsRootNode) return;
        
        RoomNode grandparent = parent.Parent;

        bool isIndicatedInGrandparent =
            grandparent.RoomType != RoomType.Boss &&
            Random.value <= MapGenerationLookup.Instance.ChanceIndicatesGrandchildRoomType;
        if (!isIndicatedInGrandparent) return;

        grandparent.ExitIdToFurtherIndicatedRoomTypes[grandparent.ChildToExitIdLookup[parent]].Add(node.RoomType);
    }
    
    private void TryGenerateVictoryNode(RoomNode node)
    {
        if (node.Depth != finalNodeDepth) return;

        RoomNode victoryNode = new RoomNode
        {
            Parent = node,
            RoomType = RoomType.Victory,
            Scene = Scene.GameplayVictoryScene
        };

        node.Children.Add(victoryNode);
    }
}
