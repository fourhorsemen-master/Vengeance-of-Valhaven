public class AbilitySelectionRoomManager : Singleton<AbilitySelectionRoomManager>
{
    public bool AbilitiesViewed { get; private set; } = false;
    public bool AbilitySelected { get; private set; } = false;

    private readonly Subject abilitiesViewedSubject = new Subject();

    private RoomNode currentRoomNode;

    public Subject AbilitySelectedSubject { get; } = new Subject();

    protected override void Awake()
    {
        base.Awake();

        currentRoomNode = PersistenceManager.Instance.SaveData.CurrentRoomNode;
        if (currentRoomNode.RoomType != RoomType.Ability) return;

        AbilitiesViewed = currentRoomNode.AbilityRoomSaveData.AbilitiesViewed;
        AbilitySelected = currentRoomNode.AbilityRoomSaveData.AbilitySelected;
    }

    private void Start()
    {
        if (currentRoomNode.RoomType != RoomType.Ability) return;

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

    public void SelectAbility(Ability2 abilityReference)
    {
        ActorCache.Instance.Player.AbilityTree.AddToInventory(abilityReference);
        AbilitySelected = true;
        AbilitySelectedSubject.Next();
        PersistenceManager.Instance.Save();
    }
}
