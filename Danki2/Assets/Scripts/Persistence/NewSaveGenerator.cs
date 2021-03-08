using System.Collections.Generic;

public static class NewSaveGenerator
{
    private const int SaveDataVersion = 1;

    public static SaveData Generate()
    {
        return new SaveData
        {
            Version = SaveDataVersion,
            PlayerHealth = 100,
            AbilityTree = AbilityTreeFactory.CreateTree(
                new EnumDictionary<AbilityReference, int>(3),
                AbilityTreeFactory.CreateNode(AbilityReference.SweepingStrike),
                AbilityTreeFactory.CreateNode(AbilityReference.Lunge)
            ),
            CurrentSceneId = 0,
            SceneSaveDataLookup = GenerateNewSceneSaveDataLookup(),
            SceneTransitions = GenerateNewSceneTransitions()
        };
    }

    private static Dictionary<int, SceneSaveData> GenerateNewSceneSaveDataLookup()
    {
        return new Dictionary<int, SceneSaveData>
        {
            [0] =
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
                    }
                },
            [1] = new SceneSaveData
            {
                Id = 1,
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
                }
            },
            [2] = new SceneSaveData
            {
                Id = 2,
                Scene = Scene.GameplayScene3,
                SceneType = SceneType.Combat,
                CombatSceneSaveData = new CombatSceneSaveData
                {
                    EnemiesCleared = false,
                    SpawnerIdToSpawnedActor = new Dictionary<int, ActorType>
                    {
                        [0] = ActorType.Bear
                    }
                }
            },
            [3] = new SceneSaveData
            {
                Id = 3,
                Scene = Scene.GameplayVictoryScene,
                SceneType = SceneType.Victory
            }
        };
    }

    private static Dictionary<int, List<int>> GenerateNewSceneTransitions()
    {
        return new Dictionary<int, List<int>>
        {
            [0] = ListUtils.Singleton(1),
            [1] = ListUtils.Singleton(2),
            [2] = ListUtils.Singleton(3)
        };
    }
}
