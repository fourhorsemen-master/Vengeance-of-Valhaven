using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public static class NewSaveGenerator
{
    private const int SaveDataVersion = 1;

    /// <summary>
    /// Generates a new save data object which can be used to start a new game.
    /// </summary>
    /// <param name="seed"> An optional seed to generate the save with </param>
    public static SaveData Generate(int seed = -1)
    {
        if (seed == -1) seed = Random.Range(0, int.MaxValue);
        Random.InitState(seed);

        RoomLayoutNode rootNode = RoomLayoutGenerator.Generate();
        int defeatRoomId = rootNode.FindMaxId() + 1;

        return new SaveData
        {
            Version = SaveDataVersion,
            PlayerHealth = 20,
            AbilityTree = AbilityTreeFactory.CreateTree(
                new EnumDictionary<AbilityReference, int>(3),
                AbilityTreeFactory.CreateNode(AbilityReference.SweepingStrike),
                AbilityTreeFactory.CreateNode(AbilityReference.Lunge)
            ),
            CurrentRoomId = 0,
            DefeatRoomId = defeatRoomId,
            RoomSaveDataLookup = GenerateRoomSaveDataLookup(rootNode, defeatRoomId)
        };
    }

    private static Dictionary<int, RoomSaveData> GenerateRoomSaveDataLookup(RoomLayoutNode rootNode, int defeatRoomId)
    {
        Dictionary<int, RoomSaveData> roomSaveDataLookup = new Dictionary<int, RoomSaveData>();

        rootNode.IterateDown(node =>
        {
            switch (node.RoomType)
            {
                case RoomType.Combat:
                    roomSaveDataLookup[node.Id] = GenerateCombatRoomSaveData(node);
                    break;
                case RoomType.Victory:
                    roomSaveDataLookup[node.Id] = GenerateVictoryRoomSaveData(node);
                    break;
            }
        });

        roomSaveDataLookup[defeatRoomId] = GenerateDefeatRoomSaveData(defeatRoomId);

        return roomSaveDataLookup;
    }

    private static RoomSaveData GenerateCombatRoomSaveData(RoomLayoutNode node)
    {
        return new RoomSaveData
        {
            Id = node.Id,
            Scene = node.Scene,
            RoomType = node.RoomType,
            CombatRoomSaveData = new CombatRoomSaveData
            {
                EnemiesCleared = false,
                SpawnerIdToSpawnedActor = node.SpawnerIdToSpawnedActor
            },
            RoomTransitionerIdToNextRoomId = node.ExitIdToChildLookup.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Id
            ),
            ModuleSeed = Random.Range(0, int.MaxValue),
            CameraOrientation = node.CameraOrientation,
            PlayerSpawnerId = node.EntranceId
        };
    }

    private static RoomSaveData GenerateVictoryRoomSaveData(RoomLayoutNode node)
    {
        return new RoomSaveData
        {
            Id = node.Id,
            Scene = node.Scene,
            RoomType = node.RoomType
        };
    }

    private static RoomSaveData GenerateDefeatRoomSaveData(int defeatRoomId)
    {
        return new RoomSaveData
        {
            Id = defeatRoomId,
            Scene = Scene.GameplayDefeatScene,
            RoomType = RoomType.Defeat
        };
    }
}
