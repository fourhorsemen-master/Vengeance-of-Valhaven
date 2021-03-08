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
            CurrentSceneId = 0,
            DefeatSceneId = 5,
            SceneSaveDataLookup = GenerateNewSceneSaveDataLookup(),
            SceneTransitions = GenerateNewSceneTransitions()
        };
    }

    private static Dictionary<int, SceneSaveData> GenerateNewSceneSaveDataLookup()
    {
        return new Dictionary<int, SceneSaveData>
        {
            {
                0,
                new SceneSaveData
                {
                    Id = 0,
                    Scene = Scene.GameplayScene1,
                    SceneType = SceneType.Combat,
                    CombatSceneSaveData = new CombatSceneSaveData
                    {
                        EnemiesCleared = false,
                        SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                        {
                            [0] = ActorType.Wolf
                        }
                    },
                    SceneTransitionerIdToNextSceneId = new Dictionary<int, int>
                    {
                        [0] = 1,
                        [1] = 2
                    }
                }
            },
            {
                1,
                new SceneSaveData
                {
                    Id = 1,
                    Scene = Scene.GameplayScene2,
                    SceneType = SceneType.Combat,
                    CombatSceneSaveData = new CombatSceneSaveData
                    {
                        EnemiesCleared = false,
                        SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                        {
                            [0] = ActorType.Wolf
                        }
                    },
                    SceneTransitionerIdToNextSceneId = new Dictionary<int, int>
                    {
                        [0] = 3
                    }
                }
            },
            {
                2,
                new SceneSaveData
                {
                    Id = 2,
                    Scene = Scene.GameplayScene2,
                    SceneType = SceneType.Combat,
                    CombatSceneSaveData = new CombatSceneSaveData
                    {
                        EnemiesCleared = false,
                        SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                        {
                            [0] = ActorType.Wolf,
                            [1] = ActorType.Wolf
                        }
                    },
                    SceneTransitionerIdToNextSceneId = new Dictionary<int, int>
                    {
                        [0] = 3
                    }
                }
            },
            {
                3,
                new SceneSaveData
                {
                    Id = 3,
                    Scene = Scene.GameplayScene3,
                    SceneType = SceneType.Combat,
                    CombatSceneSaveData = new CombatSceneSaveData
                    {
                        EnemiesCleared = false,
                        SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                        {
                            [0] = ActorType.Bear
                        }
                    },
                    SceneTransitionerIdToNextSceneId = new Dictionary<int, int>
                    {
                        [0] = 4
                    }
                }
            },
            {
                4,
                new SceneSaveData
                {
                    Id = 4,
                    Scene = Scene.GameplayVictoryScene,
                    SceneType = SceneType.Victory
                }
            },
            {
                5,
                new SceneSaveData
                {
                    Id = 5,
                    Scene = Scene.GameplayDefeatScene,
                    SceneType = SceneType.Defeat
                }
            }
        };
    }

    private static Dictionary<int, List<int>> GenerateNewSceneTransitions()
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
