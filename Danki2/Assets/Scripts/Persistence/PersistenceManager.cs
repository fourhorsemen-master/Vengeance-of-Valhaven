using System.Linq;

public class PersistenceManager : NotDestroyedOnLoadSingleton<PersistenceManager>
{
    public virtual SaveData SaveData { get; private set; }

    protected virtual void Start()
    {
        if (SaveDataManager.Instance.HasSaveData)
        {
            SaveData = SaveDataManager.Instance.Load();
            return;
        }

        SaveData = NewSaveGenerator.Generate();
        SaveDataManager.Instance.Save(SaveData);
    }

    public virtual void Save()
    {
        UpdateSaveData();
        SaveDataManager.Instance.Save(SaveData);
    }

    public virtual void TransitionToNextScene(int nextSceneId)
    {
        if (!GameplaySceneTransitionManager.Instance.CanTransition) return;
        if (!SaveData.SceneTransitions[SaveData.CurrentSceneId].Contains(nextSceneId)) return;

        UpdateSaveData();
        SaveData.CurrentSceneId = nextSceneId;
        SaveDataManager.Instance.Save(SaveData);
        SceneUtils.LoadScene(SaveData.SceneSaveDataLookup[SaveData.CurrentSceneId].Scene);
    }

    public virtual void TransitionToDefeatScene()
    {
        UpdateSaveData();
        SaveData.CurrentSceneId = SaveData.DefeatSceneId;
        SaveDataManager.Instance.Save(SaveData);
        SceneUtils.LoadScene(SaveData.SceneSaveDataLookup[SaveData.DefeatSceneId].Scene);
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
}
