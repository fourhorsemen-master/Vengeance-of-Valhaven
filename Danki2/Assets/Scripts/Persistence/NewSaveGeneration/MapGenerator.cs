using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    protected override bool DestroyOnLoad => false;
    
    public RoomNode Generate()
    {
        RoomNode rootNode = new RoomNode {Depth = 1};

        rootNode.IterateDown(GenerateChildren, node => node.Depth <= MapGenerationLookup.Instance.GeneratedRoomDepth);
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

    /// <summary>
    /// Sets the room type according to the following strategy:
    ///  - If the room is at the required depth, then set its room type to boss.
    ///  - For each room type, find the distance from the node to the nearest parent with that room type,
    ///  - If no such parent exists, assume that the node before the root node was of that room type. That is to
    ///    assume that the node before the root node is a special node of every room type,
    ///  - If there is no weight available for a room type (i.e. it's weight array does not have a value for that
    ///    room type's distance), then mark that room type as required,
    ///  - If there are any required room types, then select one randomly,
    ///  - Otherwise, select a room randomly from all room types weighted according to the relevant weight for each
    ///    room type, which depends on the distance from the node and a parent with that room type.
    /// </summary>
    private void SetRoomType(RoomNode node)
    {
        if (node.Depth == 1)
        {
            node.RoomType = RoomType.ZoneIntroduction;
            return;
        }
        
        if (node.Depth == MapGenerationLookup.Instance.MaxRoomDepth)
        {
            node.RoomType = RoomType.Boss;
            return;
        }

        Dictionary<RoomType, int> distancesFromPreviousRoomTypes = new Dictionary<RoomType, int>();
        MapGenerationLookup.Instance.ForEachRoomTypeInPool(roomType =>
        {
            int distance = node.GetDistanceFromPreviousRoomType(roomType);
            distancesFromPreviousRoomTypes[roomType] = distance == -1 ? node.Depth : distance;
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
        List<ActorType> spawnedEnemies = MapGenerationLookup.Instance.SpawnedEnemiesPerDepth[node.Depth - 1].SpawnedEnemies;
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

        bool isIndicatedInParent =
            node.RoomType == RoomType.Boss ||
            Random.value <= MapGenerationLookup.Instance.ChanceIndicatesChildRoomType;
        if (!isIndicatedInParent) return;

        RoomNode parent = node.Parent;
        parent.ExitIdToIndicatesNextRoomType[parent.ChildToExitIdLookup[node]] = true;

        if (parent.IsRootNode) return;

        bool isIndicatedInGrandparent = Random.value <= MapGenerationLookup.Instance.ChanceIndicatesGrandchildRoomType;
        if (!isIndicatedInGrandparent) return;

        RoomNode grandparent = parent.Parent;
        grandparent.ExitIdToFurtherIndicatedRoomTypes[grandparent.ChildToExitIdLookup[parent]].Add(node.RoomType);
    }
    
    private void TryGenerateVictoryNode(RoomNode node)
    {
        if (node.Depth != MapGenerationLookup.Instance.MaxRoomDepth) return;

        RoomNode victoryNode = new RoomNode
        {
            Parent = node,
            RoomType = RoomType.Victory,
            Scene = Scene.GameplayVictoryScene
        };

        node.Children.Add(victoryNode);
    }
}
