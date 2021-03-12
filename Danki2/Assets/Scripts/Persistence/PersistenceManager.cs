/// <summary>
/// Handles persistence between gameplay rooms. The public property SaveData is available for anything to read from
/// at any point so that it can initialise itself.
///
/// This singleton is persistent at all times across all gameplay rooms.
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
    /// Updates all relevant save data from the room and stores in the save data manager.
    /// </summary>
    public virtual void Save()
    {
        UpdateSaveData();
        SaveDataManager.Instance.Save(SaveData);
    }

    /// <summary>
    /// Transitions to the given room id. This room id must be a valid transition.
    ///
    /// The save data is updated before we transition, then the reference to the current room is updated so that
    /// we can save as if we were right at the start of the next room.
    /// </summary>
    public virtual void TransitionToNextRoom(int nextRoomId)
    {
        if (!GameplayRoomTransitionManager.Instance.CanTransition) return;
        if (!SaveData.RoomTransitions[SaveData.CurrentRoomId].Contains(nextRoomId)) return;

        UpdateSaveData();
        SaveData.CurrentRoomId = nextRoomId;
        SaveDataManager.Instance.Save(SaveData);
        SceneUtils.LoadScene(SaveData.CurrentRoomSaveData.Scene);
    }

    /// <summary>
    /// Saves and transitions to the special defeat room.
    /// </summary>
    public virtual void TransitionToDefeatRoom()
    {
        UpdateSaveData();
        SaveData.CurrentRoomId = SaveData.DefeatRoomId;
        SaveDataManager.Instance.Save(SaveData);
        SceneUtils.LoadScene(SaveData.DefeatRoomSaveData.Scene);
    }

    private void UpdateSaveData()
    {
        SaveData.PlayerHealth = ActorCache.Instance.Player.HealthManager.Health;
        SaveData.AbilityTree = ActorCache.Instance.Player.AbilityTree;

        RoomSaveData currentRoomSaveData = SaveData.CurrentRoomSaveData;
        if (currentRoomSaveData.RoomType == RoomType.Combat)
        {
            CombatRoomSaveData combatRoomSaveData = currentRoomSaveData.CombatRoomSaveData;
            combatRoomSaveData.EnemiesCleared = CombatRoomManager.Instance.EnemiesCleared;
        }
    }
}
