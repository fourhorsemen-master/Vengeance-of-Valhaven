/// <summary>
/// Handles persistence between gameplay rooms. The public property SaveData is available for anything to read from
/// at any point so that it can initialise itself.
///
/// This singleton is persistent at all times across all gameplay rooms.
///
/// NOTE: Public items in this class are marked as virtual so that the dev persistence manager can freely override them.
/// </summary>
public class PersistenceManager : Singleton<PersistenceManager>
{
    public virtual SaveData SaveData { get; private set; }

    protected override bool DestroyOnLoad => false;

    protected virtual void Start()
    {
        if (SaveDataManager.Instance.HasSaveData)
        {
            SaveData = SaveDataManager.Instance.Load();
            return;
        }

        SaveData = NewSaveGenerator.Instance.Generate();
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
    /// Transitions to the given room id.
    ///
    /// The save data is updated before we transition, then the reference to the current room is updated so that
    /// we can save as if we were right at the start of the next room.
    /// </summary>
    public virtual void TransitionToNextRoom(RoomNode nextRoomNode)
    {
        UpdateSaveData();
        SaveData.CurrentRoomNode = nextRoomNode;
        NewSaveGenerator.Instance.GenerateNextLayer(SaveData);
        SaveDataManager.Instance.Save(SaveData);
        SceneUtils.LoadScene(SaveData.CurrentRoomNode.Scene);
    }

    /// <summary>
    /// Saves and transitions to the special defeat room.
    /// </summary>
    public virtual void TransitionToDefeatRoom()
    {
        UpdateSaveData();
        SaveData.CurrentRoomNode = SaveData.DefeatRoom;
        SaveDataManager.Instance.Save(SaveData);
        SceneUtils.LoadScene(SaveData.DefeatRoom.Scene);
    }

    private void UpdateSaveData()
    {
        SaveData.PlayerHealth = ActorCache.Instance.Player.HealthManager.Health;
        SaveData.SerializableAbilityTree = ActorCache.Instance.Player.AbilityTree.Serialize();
        SaveData.RuneSockets = ActorCache.Instance.Player.RuneManager.RuneSockets;
        SaveData.CurrencyAmount = ActorCache.Instance.Player.CurrencyManager.CurrencyAmount;

        RoomNode currentRoomNode = SaveData.CurrentRoomNode;
        switch (currentRoomNode.RoomType)
        {
            case RoomType.Combat:
            case RoomType.Boss:
                CombatRoomSaveData combatRoomSaveData = currentRoomNode.CombatRoomSaveData;
                combatRoomSaveData.EnemiesCleared = CombatRoomManager.Instance.EnemiesCleared;
                break;
            case RoomType.Ability:
                AbilityRoomSaveData abilityRoomSaveData = currentRoomNode.AbilityRoomSaveData;
                abilityRoomSaveData.AbilitiesViewed = AbilitySelectionRoomManager.Instance.AbilitiesViewed;
                abilityRoomSaveData.AbilitySelected = AbilitySelectionRoomManager.Instance.AbilitySelected;
                break;
            case RoomType.Healing:
                currentRoomNode.HealingRoomSaveData.HasHealed = HealingRoomManager.Instance.HasHealed;
                break;
            case RoomType.Rune:
                RuneRoomSaveData runeRoomSaveData = currentRoomNode.RuneRoomSaveData;
                runeRoomSaveData.RunesViewed = RuneRoomManager.Instance.RunesViewed;
                runeRoomSaveData.RuneSelected = RuneRoomManager.Instance.RuneSelected;
                break;
        }
    }
}
