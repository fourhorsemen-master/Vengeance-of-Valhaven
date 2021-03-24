public class AbilityRoomManager : Singleton<AbilityRoomManager>
{
    public bool AbilitiesViewed { get; private set; } = false;
    public bool AbilitySelected { get; private set; } = false;

    private readonly Subject abilitiesViewedSubject = new Subject();

    public Subject AbilitySelectedSubject { get; } = new Subject();

    private void Start()
    {
        RoomSaveData roomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        if (roomSaveData.RoomType != RoomType.Ability) return;

        AbilitiesViewed = roomSaveData.AbilityRoomSaveData.AbilitiesViewed;
        AbilitySelected = roomSaveData.AbilityRoomSaveData.AbilitySelected;

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

    // private void Update()
    // {
    //     if (PersistenceManager.Instance.SaveData.CurrentRoomSaveData.RoomType != RoomType.Ability || AbilitiesViewed) return;
    //
    //     List<AbilityReference> choices = PersistenceManager.Instance.SaveData.CurrentRoomSaveData.AbilityRoomSaveData.AbilityChoices;
    //
    //     if (Input.GetKeyDown(KeyCode.Alpha1))
    //     {
    //         ActorCache.Instance.Player.AbilityTree.AddToInventory(choices[0]);
    //         AbilitiesViewed = true;
    //         abilitiesViewedSubject.Next();
    //         PersistenceManager.Instance.Save();
    //         return;
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.Alpha2))
    //     {
    //         ActorCache.Instance.Player.AbilityTree.AddToInventory(choices[1]);
    //         AbilitiesViewed = true;
    //         abilitiesViewedSubject.Next();
    //         PersistenceManager.Instance.Save();
    //         return;
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.Alpha3))
    //     {
    //         ActorCache.Instance.Player.AbilityTree.AddToInventory(choices[2]);
    //         AbilitiesViewed = true;
    //         abilitiesViewedSubject.Next();
    //         PersistenceManager.Instance.Save();
    //     }
    // }
}
