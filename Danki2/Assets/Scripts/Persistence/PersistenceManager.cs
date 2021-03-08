using System.Collections.Generic;

public class PersistenceManager : NotDestroyedOnLoadSingleton<PersistenceManager>
{
    private const int SaveDataVersion = 1;
    
    public virtual SaveData SaveData { get; private set; }

    protected virtual void Start()
    {
        if (SaveDataManager.Instance.HasSaveData)
        {
            SaveData = SaveDataManager.Instance.Load();
            return;
        }

        SaveData = GenerateNewSaveData();
        SaveDataManager.Instance.Save(SaveData);
    }

    public virtual void Save()
    {
        UpdateSaveData();
        SaveDataManager.Instance.Save(SaveData);
    }

    public virtual void TransitionToNextScene()
    {
        if (!GameplaySceneTransitionManager.Instance.CanTransition) return;
        UpdateSaveData();
        SaveData.CurrentSceneId++;
        SaveDataManager.Instance.Save(SaveData);
        SceneUtils.LoadScene(SaveData.SceneSaveDataLookup[SaveData.CurrentSceneId].Scene);
    }

    private void UpdateSaveData()
    {
        SaveData.PlayerHealth = ActorCache.Instance.Player.HealthManager.Health;
        SaveData.AbilityTree = ActorCache.Instance.Player.AbilityTree;

        SceneSaveData currentSceneSaveData = SaveData.SceneSaveDataLookup[SaveData.CurrentSceneId];
        if (currentSceneSaveData.SceneType == SceneType.Combat)
        {
            CombatSceneSaveData combatSceneSaveData = currentSceneSaveData.CombatSceneSaveData;
            combatSceneSaveData.EnemiesCleared = CombatSceneManager.Instance.EnemiesCleared;
        }
    }
    
    private SaveData GenerateNewSaveData()
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

    private Dictionary<int, SceneSaveData> GenerateNewSceneSaveDataLookup()
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

    Dictionary<int, List<int>> GenerateNewSceneTransitions()
    {
        return new Dictionary<int, List<int>>
        {
            [0] = ListUtils.Singleton(1),
            [1] = ListUtils.Singleton(2),
            [2] = ListUtils.Singleton(3)
        };
    }
}
