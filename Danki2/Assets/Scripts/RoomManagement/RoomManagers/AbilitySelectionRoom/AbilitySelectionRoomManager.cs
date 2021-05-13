public class AbilitySelectionRoomManager : Singleton<AbilitySelectionRoomManager>
{
    public bool AbilitiesViewed { get; private set; } = false;
    public bool AbilitySelected { get; private set; } = false;

    private readonly Subject abilitiesViewedSubject = new Subject();

    private RoomSaveData currentRoomSaveData;

    public Subject AbilitySelectedSubject { get; } = new Subject();

    protected override void Awake()
    {
        base.Awake();

        currentRoomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        if (currentRoomSaveData.RoomType != RoomType.Ability) return;

        AbilitiesViewed = currentRoomSaveData.AbilityRoomSaveData.AbilitiesViewed;
        AbilitySelected = currentRoomSaveData.AbilityRoomSaveData.AbilitySelected;
    }

    private void Start()
    {
        if (currentRoomSaveData.RoomType != RoomType.Ability) return;

        GameplayRoomTransitionManager.Instance.RegisterCanTransitionSubject(abilitiesViewedSubject);

        if (AbilitiesViewed) abilitiesViewedSubject.Next();
    }

    public void ViewAbilities()
    {
        if (AbilitiesViewed) return;

        AbilitiesViewed = true;
        abilitiesViewedSubject.Next();
        PersistenceManager.Instance.Save();
    }

    public void SelectAbility(AbilityReference abilityReference)
    {
        ActorCache.Instance.Player.AbilityTree.AddToInventory(abilityReference);
        AbilitySelected = true;
        AbilitySelectedSubject.Next();
        PersistenceManager.Instance.Save();
    }
}
