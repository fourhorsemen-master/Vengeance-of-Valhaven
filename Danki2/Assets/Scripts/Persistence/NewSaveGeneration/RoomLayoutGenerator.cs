using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGenerator : Singleton<RoomLayoutGenerator>
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

    public RoomLayoutNode Generate()
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

    private void RecursivelyGenerateChildren(RoomLayoutNode node, int currentDepth)
    {
        int numberOfChildren = Random.Range(minRoomExits, maxRoomExits + 1);

        for (int i = 0; i < numberOfChildren; i++)
        {
            RoomLayoutNode childNode = new RoomLayoutNode();
            node.Children.Add(childNode);

            int nextDepth = currentDepth + 1;
            if (ShouldGenerateChildren(nextDepth)) RecursivelyGenerateChildren(childNode, nextDepth);
        }
    }

    private bool ShouldGenerateChildren(int nodeDepth)
    {
        if (nodeDepth < minRoomDepth) return true;
        if (nodeDepth >= maxRoomDepth) return false;
        return Random.value < 1f / (maxRoomDepth - nodeDepth + 1);
    }

    private void SetIds(RoomLayoutNode rootNode)
    {
        int currentId = 0;
        rootNode.IterateDown(n =>
        {
            n.Id = currentId;
            currentId++;
        });
    }

    private void SetParentReferences(RoomLayoutNode rootNode)
    {
        rootNode.IterateDown(n => n.Children.ForEach(c => c.Parent = n));
    }

    private void SetRoomTypes(RoomLayoutNode rootNode)
    {
        rootNode.IterateDown(n => n.RoomType = RoomType.Combat);
    }

    private void AddVictoryNode(RoomLayoutNode rootNode)
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

    private void SetSceneData(RoomLayoutNode rootNode)
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

    private void SetSceneData(RoomLayoutNode node, Pole trueEntranceDirection)
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

        node.SpawnerIdToSpawnedActor[0] = node.Children[0].RoomType == RoomType.Victory
            ? ActorType.Bear
            : ActorType.Wolf;
    }
}
