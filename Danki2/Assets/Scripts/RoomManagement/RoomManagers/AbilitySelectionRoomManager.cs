public class AbilitySelectionRoomManager : Singleton<AbilitySelectionRoomManager>
{
    public bool AbilitiesViewed { get; private set; } = false;
    public bool AbilitySelected { get; private set; } = false;

    private readonly Subject abilitiesViewedSubject = new Subject();

    public Subject AbilitySelectedSubject { get; } = new Subject();

    protected override void Awake()
    {
        base.Awake();

        RoomSaveData roomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        if (roomSaveData.RoomType != RoomType.Ability) return;

        AbilitiesViewed = roomSaveData.AbilityRoomSaveData.AbilitiesViewed;
        AbilitySelected = roomSaveData.AbilityRoomSaveData.AbilitySelected;
    }

    private void Start()
    {
        if (PersistenceManager.Instance.SaveData.CurrentRoomSaveData.RoomType != RoomType.Ability) return;

        GameplayRoomTransitionManager.Instance.RegisterCanTransitionSubject(abilitiesViewedSubject);

        if (AbilitiesViewed) abilitiesViewedSubject.Next();
    }

    public void ViewAbilities()
    {
        if (!AbilitiesViewed)
        {
            AbilitiesViewed = true;
            abilitiesViewedSubject.Next();
            PersistenceManager.Instance.Save();
        }

        GameplayStateController.Instance.GameplayState = GameplayState.InAbilitySelection;
    }

    public void SkipAbilities()
    {
        GameplayStateController.Instance.GameplayState = GameplayState.Playing;
    }

    public void SelectAbility(AbilityReference abilityReference)
    {
        ActorCache.Instance.Player.AbilityTree.AddToInventory(abilityReference);
        AbilitySelected = true;
        AbilitySelectedSubject.Next();
        PersistenceManager.Instance.Save();

        GameplayStateController.Instance.GameplayState = GameplayState.Playing;
    }
}
