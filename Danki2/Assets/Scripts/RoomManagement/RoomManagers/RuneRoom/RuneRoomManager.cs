public class RuneRoomManager : Singleton<RuneRoomManager>
{
    public bool RunesViewed { get; private set; } = false;
    public bool RuneSelected { get; private set; } = false;

    private readonly Subject runesViewedSubject = new Subject();

    public Subject RuneSelectedSubject { get; } = new Subject();

    protected override void Awake()
    {
        base.Awake();

        RoomSaveData roomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        if (roomSaveData.RoomType != RoomType.Rune) return;

        RunesViewed = roomSaveData.RuneRoomSaveData.RunesViewed;
        RuneSelected = roomSaveData.RuneRoomSaveData.RuneSelected;
    }

    private void Start()
    {
        if (PersistenceManager.Instance.SaveData.CurrentRoomSaveData.RoomType != RoomType.Rune) return;

        GameplayRoomTransitionManager.Instance.RegisterCanTransitionSubject(runesViewedSubject);

        if (RunesViewed) runesViewedSubject.Next();
    }

    public void ViewRunes()
    {
        if (RunesViewed) return;

        RunesViewed = true;
        runesViewedSubject.Next();
        PersistenceManager.Instance.Save();
    }

    public void SelectRune(Rune rune)
    {
        // TODO: actually add the rune...
        RuneSelected = true;
        RuneSelectedSubject.Next();
        PersistenceManager.Instance.Save();
    }
}
