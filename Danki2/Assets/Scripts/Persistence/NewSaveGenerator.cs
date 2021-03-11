using System.Collections.Generic;

public static class NewSaveGenerator
{
    private const int SaveDataVersion = 1;

    /// <summary>
    /// Generates a new save data object which can be used to start a new game.
    /// </summary>
    public static SaveData Generate()
    {
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
            DefeatRoomId = 5,
            RoomSaveDataLookup = GenerateNewRoomSaveDataLookup(),
            RoomTransitions = GenerateNewRoomTransitions()
        };
    }

    private static Dictionary<int, RoomSaveData> GenerateNewRoomSaveDataLookup()
    {
        return new Dictionary<int, RoomSaveData>
        {
            {
                0,
                new RoomSaveData
                {
                    Id = 0,
                    Scene = Scene.GameplayScene1,
                    RoomType = RoomType.Combat,
                    CombatRoomSaveData = new CombatRoomSaveData
                    {
                        EnemiesCleared = false,
                        SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                        {
                            [0] = ActorType.Wolf
                        }
                    },
                    RoomTransitionerIdToNextRoomId = new Dictionary<int, int>
                    {
                        [0] = 1,
                        [1] = 2
                    },
                    ModuleSeed = 1000
                }
            },
            {
                1,
                new RoomSaveData
                {
                    Id = 1,
                    Scene = Scene.GameplayScene2,
                    RoomType = RoomType.Combat,
                    CombatRoomSaveData = new CombatRoomSaveData
                    {
                        EnemiesCleared = false,
                        SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                        {
                            [0] = ActorType.Wolf
                        }
                    },
                    RoomTransitionerIdToNextRoomId = new Dictionary<int, int>
                    {
                        [0] = 3
                    },
                    ModuleSeed = 2000
                }
            },
            {
                2,
                new RoomSaveData
                {
                    Id = 2,
                    Scene = Scene.GameplayScene2,
                    RoomType = RoomType.Combat,
                    CombatRoomSaveData = new CombatRoomSaveData
                    {
                        EnemiesCleared = false,
                        SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                        {
                            [0] = ActorType.Wolf,
                            [1] = ActorType.Wolf
                        }
                    },
                    RoomTransitionerIdToNextRoomId = new Dictionary<int, int>
                    {
                        [0] = 3
                    },
                    ModuleSeed = 3000
                }
            },
            {
                3,
                new RoomSaveData
                {
                    Id = 3,
                    Scene = Scene.GameplayScene3,
                    RoomType = RoomType.Combat,
                    CombatRoomSaveData = new CombatRoomSaveData
                    {
                        EnemiesCleared = false,
                        SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                        {
                            [0] = ActorType.Bear
                        }
                    },
                    RoomTransitionerIdToNextRoomId = new Dictionary<int, int>
                    {
                        [0] = 4
                    },
                    ModuleSeed = 4000
                }
            },
            {
                4,
                new RoomSaveData
                {
                    Id = 4,
                    Scene = Scene.GameplayVictoryScene,
                    RoomType = RoomType.Victory
                }
            },
            {
                5,
                new RoomSaveData
                {
                    Id = 5,
                    Scene = Scene.GameplayDefeatScene,
                    RoomType = RoomType.Defeat
                }
            }
        };
    }

    private static Dictionary<int, List<int>> GenerateNewRoomTransitions()
    {
        return new Dictionary<int, List<int>>
        {
            {0, new List<int> {1, 2}},
            {1, new List<int> {3}},
            {2, new List<int> {3}},
            {3, new List<int> {4}}
        };
    }
}
