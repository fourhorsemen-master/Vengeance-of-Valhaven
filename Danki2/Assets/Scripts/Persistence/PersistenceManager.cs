/// <summary>
/// Handles persistence between gameplay scenes. The public property SaveData is available for anything to read from
/// at any point so that it can initialise itself.
///
/// This singleton is persistent at all times across all gameplay scenes.
///
/// NOTE: Public items in this class are marked as virtual so that the dev persistence manager can freely override them.
/// </summary>
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

    /// <summary>
    /// Updates all relevant save data from the scene and stores in the save data manager.
    /// </summary>
    public virtual void Save()
    {
        UpdateSaveData();
        SaveDataManager.Instance.Save(SaveData);
    }

    /// <summary>
    /// Transitions to the given scene id. This scene id must be a valid transition.
    ///
    /// The save data is updated before we transition, then the reference to the current scene is updated so that
    /// we can save as if we were right at the start of the next scene.
    /// </summary>
    public virtual void TransitionToNextScene(int nextSceneId)
    {
        if (!GameplaySceneTransitionManager.Instance.CanTransition) return;
        if (!SaveData.SceneTransitions[SaveData.CurrentSceneId].Contains(nextSceneId)) return;

        UpdateSaveData();
        SaveData.CurrentSceneId = nextSceneId;
        SaveDataManager.Instance.Save(SaveData);
        SceneUtils.LoadScene(SaveData.SceneSaveDataLookup[SaveData.CurrentSceneId].Scene);
    }

    /// <summary>
    /// Saves and transitions to the special defeat scene.
    /// </summary>
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
