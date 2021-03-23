using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class NewSaveGenerator : Singleton<NewSaveGenerator>
{
    private const int SaveDataVersion = 0;

    protected override bool DestroyOnLoad => false;

    /// <summary>
    /// Generates a new save data object which can be used to start a new game.
    /// </summary>
    /// <param name="seed"> An optional seed to generate the save with </param>
    public SaveData Generate(int seed = -1)
    {
        if (seed == -1) seed = Random.Range(0, int.MaxValue);
        Random.InitState(seed);

        MapNode rootNode = MapGenerator.Instance.Generate();
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

    private Dictionary<int, RoomSaveData> GenerateRoomSaveDataLookup(MapNode rootNode, int defeatRoomId)
    {
        Dictionary<int, RoomSaveData> roomSaveDataLookup = new Dictionary<int, RoomSaveData>();

        rootNode.IterateDown(node =>
        {
            switch (node.RoomType)
            {
                case RoomType.Combat:
                case RoomType.Boss:
                    roomSaveDataLookup[node.Id] = GenerateCombatRoomSaveData(node);
                    break;
                case RoomType.Ability:
                    roomSaveDataLookup[node.Id] = GenerateAbilityRoomSaveData(node);
                    break;
                case RoomType.Victory:
                    roomSaveDataLookup[node.Id] = GenerateVictoryRoomSaveData(node);
                    break;
            }
        });

        roomSaveDataLookup[defeatRoomId] = GenerateDefeatRoomSaveData(defeatRoomId);

        return roomSaveDataLookup;
    }

    private RoomSaveData GenerateCombatRoomSaveData(MapNode node)
    {
        RoomSaveData roomSaveData = GenerateCommonRoomSaveData(node);
        roomSaveData.CombatRoomSaveData = new CombatRoomSaveData
        {
            EnemiesCleared = false,
            SpawnerIdToSpawnedActor = node.SpawnerIdToSpawnedActor
        };

        return roomSaveData;
    }

    private RoomSaveData GenerateAbilityRoomSaveData(MapNode node)
    {
        RoomSaveData roomSaveData = GenerateCommonRoomSaveData(node);
        roomSaveData.AbilityRoomSaveData = new AbilityRoomSaveData
        {
            AbilityChoices = node.AbilityChoices
        };

        return roomSaveData;
    }

    private RoomSaveData GenerateCommonRoomSaveData(MapNode node)
    {
        return new RoomSaveData()
        {
            Id = node.Id,
            Scene = node.Scene,
            RoomType = node.RoomType,
            RoomTransitionerIdToNextRoomId = node.ExitIdToChildLookup.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Id
            ),
            ModuleSeed = Random.Range(0, int.MaxValue),
            TransitionModuleSeed = Random.Range(0, int.MaxValue),
            CameraOrientation = node.CameraOrientation,
            PlayerSpawnerId = node.EntranceId
        };
    }
    
    private RoomSaveData GenerateVictoryRoomSaveData(MapNode node)
    {
        return new RoomSaveData
        {
            Id = node.Id,
            Scene = node.Scene,
            RoomType = node.RoomType
        };
    }

    private RoomSaveData GenerateDefeatRoomSaveData(int defeatRoomId)
    {
        return new RoomSaveData
        {
            Id = defeatRoomId,
            Scene = Scene.GameplayDefeatScene,
            RoomType = RoomType.Defeat
        };
    }
}
