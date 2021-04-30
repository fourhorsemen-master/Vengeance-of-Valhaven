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
        if (seed == -1) seed = RandomUtils.Seed();
        Random.InitState(seed);

        MapNode rootNode = MapGenerator.Instance.Generate();
        int defeatRoomId = rootNode.FindMaxId() + 1;

        EnumDictionary<AbilityReference, int> ownedAbilities = new EnumDictionary<AbilityReference, int>(0);
        ownedAbilities[AbilityReference.Slash] = 1;
        ownedAbilities[AbilityReference.Lunge] = 1;

        List<RuneSocket> runeSockets = new List<RuneSocket>();
        Utils.Repeat(MapGenerationLookup.Instance.RuneSockets, () => runeSockets.Add(new RuneSocket()));
        
        return new SaveData
        {
            Version = SaveDataVersion,
            Seed = seed,
            PlayerHealth = 20,
            SerializableAbilityTree = AbilityTreeFactory.CreateTree(
                ownedAbilities,
                AbilityTreeFactory.CreateNode(AbilityReference.Slash),
                AbilityTreeFactory.CreateNode(AbilityReference.Lunge)
            ).Serialize(),
            RuneSockets = runeSockets,
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
                case RoomType.Healing:
                    roomSaveDataLookup[node.Id] = GenerateHealingRoomSaveData(node);
                    break;
                case RoomType.Rune:
                    roomSaveDataLookup[node.Id] = GenerateRuneRoomSaveData(node);
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

    private RoomSaveData GenerateHealingRoomSaveData(MapNode node)
    {
        RoomSaveData roomSaveData = GenerateCommonRoomSaveData(node);
        roomSaveData.HealingRoomSaveData = new HealingRoomSaveData();

        return roomSaveData;
    }

    private RoomSaveData GenerateRuneRoomSaveData(MapNode node)
    {
        RoomSaveData roomSaveData = GenerateCommonRoomSaveData(node);
        roomSaveData.RuneRoomSaveData = new RuneRoomSaveData
        {
            RunesViewed = false,
            RuneSelected = false
        };

        return roomSaveData;
    }

    private RoomSaveData GenerateCommonRoomSaveData(MapNode node)
    {
        return new RoomSaveData
        {
            Id = node.Id,
            Depth = node.Depth,
            Scene = node.Scene,
            RoomType = node.RoomType,
            RoomTransitionerIdToTransitionData = node.ExitIdToChildLookup.ToDictionary(
                kvp => kvp.Key,
                kvp => new TransitionData
                {
                    NextRoomId = kvp.Value.Id,
                    IndicatesNextRoomType = node.ExitIdToIndicatesNextRoomType[kvp.Key],
                    FurtherIndicatedRoomTypes = node.ExitIdToFurtherIndicatedRoomTypes[kvp.Key]
                }
            ),
            ModuleSeed = RandomUtils.Seed(),
            TransitionModuleSeed = RandomUtils.Seed(),
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
