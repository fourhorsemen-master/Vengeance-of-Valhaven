using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : Singleton<MapGenerator>
{
    [SerializeField] private int minRoomExits = 0;
    [SerializeField] private int maxRoomExits = 0;
    [SerializeField] private int minRoomDepth = 0;
    [SerializeField] private int maxRoomDepth = 0;

    protected override bool DestroyOnLoad => false;

    private static readonly Dictionary<Pole, Pole> reversedPoleLookup = new Dictionary<Pole, Pole>
    {
        [Pole.North] = Pole.South,
        [Pole.East] = Pole.West,
        [Pole.South] = Pole.North,
        [Pole.West] = Pole.East,
    };

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

        int numberOfChildren = Random.Range(minRoomExits, maxRoomExits + 1);

        for (int i = 0; i < numberOfChildren; i++)
        {
            MapNode childNode = new MapNode();
            node.Children.Add(childNode);

            GenerateChildren(childNode, currentDepth + 1);
        }
    }

    private bool ShouldGenerateChildren(int depth)
    {
        if (depth < minRoomDepth) return true;
        if (depth >= maxRoomDepth) return false;
        return Random.value <= 1f / (maxRoomDepth - depth + 1);
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
        rootNode.IterateDown(n => n.RoomType = n.IsLeafNode ? RoomType.Boss : RoomType.Combat);
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
                SetSceneData(node, reversedPoleLookup[trueParentExitDirection]);
            },
            node => !node.IsRootNode && node.RoomType != RoomType.Victory
        );
    }

    private void SetSceneData(MapNode node, Pole trueEntranceDirection)
    {
        node.Scene = RandomUtils.Choice(SceneLookup.Instance.GetValidScenes(trueEntranceDirection, node.Children.Count));
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
            validExitIds.Remove(exitId);
        });

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
}
