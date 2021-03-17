using System.Collections.Generic;
using UnityEngine;

public static class RoomLayoutGenerator
{
    private const int MinRoomExits = 1;
    private const int MaxRoomExits = 3;
    private const int MinRoomDepth = 2;
    private const int MaxRoomDepth = 3;

    private static readonly Dictionary<Pole, Pole> reversedPoleLookup = new Dictionary<Pole, Pole>
    {
        [Pole.North] = Pole.South,
        [Pole.East] = Pole.West,
        [Pole.South] = Pole.North,
        [Pole.West] = Pole.East,
    };

    public static RoomLayoutNode Generate()
    {
        RoomLayoutNode rootNode = new RoomLayoutNode();
        RecursivelyGenerateChildren(rootNode, 1);
        SetIds(rootNode);
        SetParentReferences(rootNode);
        SetRoomTypes(rootNode);
        AddVictoryNode(rootNode);
        SetSceneData(rootNode);

        return rootNode;
    }

    private static void RecursivelyGenerateChildren(RoomLayoutNode node, int currentDepth)
    {
        int numberOfChildren = Random.Range(MinRoomExits, MaxRoomExits + 1);

        for (int i = 0; i < numberOfChildren; i++)
        {
            RoomLayoutNode childNode = new RoomLayoutNode();
            node.Children.Add(childNode);

            int nextDepth = currentDepth + 1;
            if (ShouldGenerateChildren(nextDepth)) RecursivelyGenerateChildren(childNode, nextDepth);
        }
    }

    private static bool ShouldGenerateChildren(int nodeDepth)
    {
        if (nodeDepth < MinRoomDepth) return true;
        if (nodeDepth >= MaxRoomDepth) return false;
        return Random.value < 1f / (MaxRoomDepth - nodeDepth + 1);
    }

    private static void SetIds(RoomLayoutNode rootNode)
    {
        int currentId = 0;
        rootNode.IterateDown(n =>
        {
            n.Id = currentId;
            currentId++;
        });
    }

    private static void SetParentReferences(RoomLayoutNode rootNode)
    {
        rootNode.IterateDown(n => n.Children.ForEach(c => c.Parent = n));
    }

    private static void SetRoomTypes(RoomLayoutNode rootNode)
    {
        rootNode.IterateDown(n => n.RoomType = RoomType.Combat);
    }

    private static void AddVictoryNode(RoomLayoutNode rootNode)
    {
        RoomLayoutNode victoryNode = new RoomLayoutNode
        {
            Id = rootNode.FindMaxId() + 1,
            RoomType = RoomType.Victory,
            Scene = Scene.GameplayVictoryScene
        };

        List<RoomLayoutNode> leafNodes = new List<RoomLayoutNode>();

        rootNode.IterateDown(n =>
        {
            if (n.IsLeafNode) leafNodes.Add(n);
        });

        leafNodes.ForEach(n => n.Children.Add(victoryNode));
    }

    private static void SetSceneData(RoomLayoutNode rootNode)
    {
        SetSceneData(rootNode, Pole.South);

        rootNode.IterateDown(
            node =>
            {
                Pole parentExitDirection = SceneLookup.Instance.GetTrueExitDirection(
                    node.Parent.Scene,
                    node.Parent.CameraOrientation,
                    node.Parent.ChildToExitIdLookup[node]
                );
                SetSceneData(node, reversedPoleLookup[parentExitDirection]);
            },
            node => !node.IsRootNode && node.RoomType != RoomType.Victory
        );
    }

    private static void SetSceneData(RoomLayoutNode node, Pole entranceDirection)
    {
        node.Scene = RandomUtils.Choice(SceneLookup.Instance.GetValidScenes(entranceDirection, node.Children.Count));
        node.CameraOrientation = RandomUtils.Choice(SceneLookup.Instance.GetValidCameraOrientations(
            node.Scene,
            entranceDirection,
            node.Children.Count
        ));
        node.EntranceId = RandomUtils.Choice(SceneLookup.Instance.GetValidEntranceIds(
            node.Scene,
            entranceDirection,
            node.CameraOrientation
        ));

        List<ExitData> validExits = SceneLookup.Instance.GetValidExits(
            node.Scene,
            node.CameraOrientation,
            node.EntranceId
        );
        node.Children.ForEach(child =>
        {
            ExitData exitData = RandomUtils.Choice(validExits);
            node.ExitIdToChildLookup[exitData.Id] = child;
            node.ChildToExitIdLookup[child] = exitData.Id;
            validExits.Remove(exitData);
        });

        node.SpawnerIdToSpawnedActor[0] = node.Children[0].RoomType == RoomType.Victory
            ? ActorType.Bear
            : ActorType.Wolf;
    }
}
