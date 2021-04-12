using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MapGenerator : Singleton<MapGenerator>
{
    protected override bool DestroyOnLoad => false;

    public MapNode Generate()
    {
        MapNode rootNode = new MapNode();
        GenerateChildren(rootNode, 1);
        SetIds(rootNode);
        SetParentReferences(rootNode);
        SetRoomTypes(rootNode);
        AddVictoryNode(rootNode);
        SetSceneData(rootNode);

        return rootNode;
    }

    private void GenerateChildren(MapNode node, int currentDepth)
    {
        if (!ShouldGenerateChildren(currentDepth)) return;

        int numberOfChildren = Random.Range(
            MapGenerationLookup.Instance.MinRoomExits,
            MapGenerationLookup.Instance.MaxRoomExits + 1
        );

        for (int i = 0; i < numberOfChildren; i++)
        {
            MapNode childNode = new MapNode();
            node.Children.Add(childNode);

            GenerateChildren(childNode, currentDepth + 1);
        }
    }

    private bool ShouldGenerateChildren(int depth)
    {
        if (depth < MapGenerationLookup.Instance.MinRoomDepth) return true;
        if (depth >= MapGenerationLookup.Instance.MaxRoomDepth) return false;
        return Random.value <= 1f / (MapGenerationLookup.Instance.MaxRoomDepth - depth + 1);
    }

    private void SetIds(MapNode rootNode)
    {
        int currentId = 0;
        rootNode.IterateDown(n =>
        {
            n.Id = currentId;
            currentId++;
        });
    }

    private void SetParentReferences(MapNode rootNode)
    {
        rootNode.IterateDown(n => n.Children.ForEach(c => c.Parent = n));
    }

    private void SetRoomTypes(MapNode rootNode)
    {
        rootNode.IterateDown(node =>
        {
            if (node.IsLeafNode)
            {
                node.RoomType = RoomType.Boss;
                return;
            }

            SetRoomType(node);
        });
    }

    /// <summary>
    /// Sets the room type according to the following strategy:
    ///  - For each room type, find the distance from the node to the nearest parent with that room type,
    ///  - If no such parent exists, assume that the node before the root node was of that room type. That is to
    ///    assume that the node before the root node is a special node of every room type,
    ///  - If there is no weight available for a room type (i.e. it's weight array does not have a value for that
    ///    room type's distance), then mark that room type as required,
    ///  - If there are any required room types, then select one randomly,
    ///  - Otherwise, select a room randomly from all room types weighted according to the relevant weight for each
    ///    room type, which depends on the distance from the node and a parent with that room type.
    /// </summary>
    private void SetRoomType(MapNode node)
    {
        Dictionary<RoomType, int> distancesFromPreviousRoomTypes = new Dictionary<RoomType, int>();
        MapGenerationLookup.Instance.ForEachRoomTypeInPool(roomType =>
        {
            int distance = node.GetDistanceFromPreviousRoomType(roomType);
            distancesFromPreviousRoomTypes[roomType] = distance == -1 ? node.Depth : distance;
        });

        List<RoomType> requiredRoomTypes = new List<RoomType>();
        MapGenerationLookup.Instance.ForEachRoomTypeInPool(roomType =>
        {
            if (distancesFromPreviousRoomTypes[roomType] >= MapGenerationLookup.Instance.GetDistanceWhenRequired(roomType)) requiredRoomTypes.Add(roomType);
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

    private void AddVictoryNode(MapNode rootNode)
    {
        MapNode victoryNode = new MapNode
        {
            Id = rootNode.FindMaxId() + 1,
            RoomType = RoomType.Victory,
            Scene = Scene.GameplayVictoryScene
        };

        List<MapNode> leafNodes = new List<MapNode>();

        rootNode.IterateDown(n =>
        {
            if (n.IsLeafNode) leafNodes.Add(n);
        });

        leafNodes.ForEach(n => n.Children.Add(victoryNode));
    }

    private void SetSceneData(MapNode rootNode)
    {
        SetSceneData(rootNode, Pole.South);

        rootNode.IterateDown(
            node =>
            {
                Pole trueParentExitDirection = SceneLookup.Instance.GetTrueExitDirection(
                    node.Parent.Scene,
                    node.Parent.CameraOrientation,
                    node.Parent.ChildToExitIdLookup[node]
                );
                SetSceneData(node, OrientationUtils.GetReversedPole(trueParentExitDirection));
            },
            node => !node.IsRootNode && node.RoomType != RoomType.Victory
        );
    }

    private void SetSceneData(MapNode node, Pole trueEntranceDirection)
    {
        SetCommonData(node, trueEntranceDirection);

        switch (node.RoomType)
        {
            case RoomType.Combat:
            case RoomType.Boss:
                SetCombatData(node);
                break;
            case RoomType.Ability:
                SetAbilityData(node);
                break;
        }
    }

    private void SetCommonData(MapNode node, Pole trueEntranceDirection)
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
        node.EntranceId = RandomUtils.Choice(SceneLookup.Instance.GetValidEntranceIds(
            node.Scene,
            trueEntranceDirection,
            node.CameraOrientation
        ));

        SetTransitionData(node);
    }

    private void SetTransitionData(MapNode node)
    {
        List<int> validExitIds = SceneLookup.Instance.GetValidExitIds(
            node.Scene,
            node.CameraOrientation,
            node.EntranceId
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

        SetTransitionIndicationData(node);
    }

    private void SetTransitionIndicationData(MapNode node)
    {
        if (node.IsRootNode) return;

        bool isIndicatedInParent = Random.value <= MapGenerationLookup.Instance.ChanceRoomTypeIndicatedByParent;
        if (!isIndicatedInParent) return;

        MapNode parent = node.Parent;
        parent.ExitIdToIndicatesNextRoomType[parent.ChildToExitIdLookup[node]] = true;

        if (parent.IsRootNode) return;

        bool isIndicatedInGrandparent = Random.value <= MapGenerationLookup.Instance.ChanceRoomTypeIndicatedByGrandparent;
        if (!isIndicatedInGrandparent) return;

        MapNode grandparent = parent.Parent;
        grandparent.ExitIdToFurtherIndicatedRoomTypes[grandparent.ChildToExitIdLookup[parent]].Add(node.RoomType);
    }

    private void SetCombatData(MapNode node)
    {
        switch (node.RoomType)
        {
            case RoomType.Combat:
                node.SpawnerIdToSpawnedActor[0] = ActorType.Wolf;
                break;
            case RoomType.Boss:
                node.SpawnerIdToSpawnedActor[0] = ActorType.Bear;
                break;
        }
    }

    private void SetAbilityData(MapNode node)
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
            node.AbilityChoices.Add(choice);
            choices.RemoveAll(c => c == choice);
        });
    }
}
