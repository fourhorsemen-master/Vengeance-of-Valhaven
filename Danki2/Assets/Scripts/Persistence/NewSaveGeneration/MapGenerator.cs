using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : Singleton<MapGenerator>
{
    private const int AbilityChoices = 3;

    [SerializeField] private int minRoomExits = 0;
    [SerializeField] private int maxRoomExits = 0;
    [SerializeField] private int minRoomDepth = 0;
    [SerializeField] private int maxRoomDepth = 0;

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
        rootNode.IterateDown(node =>
        {
            if (node.IsLeafNode)
            {
                node.RoomType = RoomType.Boss;
                return;
            }

            if ((node.Depth + 1) % 3 == 0)
            {
                node.RoomType = RoomType.Ability;
                return;
            }

            node.RoomType = RoomType.Combat;
        });
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

        Utils.Repeat(AbilityChoices, () =>
        {
            AbilityReference choice = RandomUtils.Choice(choices);
            node.AbilityChoices.Add(choice);
            choices.RemoveAll(c => c == choice);
        });
    }
}
