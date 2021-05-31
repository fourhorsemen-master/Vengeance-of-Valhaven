public class RuneRoomManager : Singleton<RuneRoomManager>
{
    public bool RunesViewed { get; private set; } = false;
    public bool RuneSelected { get; private set; } = false;

    private readonly Subject runesViewedSubject = new Subject();

    private RoomNode currentRoomNode;

    public Subject RuneSelectedSubject { get; } = new Subject();

    protected override void Awake()
    {
        base.Awake();

        currentRoomNode = PersistenceManager.Instance.SaveData.CurrentRoomNode;
        if (currentRoomNode.RoomType != RoomType.Rune) return;

        RunesViewed = currentRoomNode.RuneRoomSaveData.RunesViewed;
        RuneSelected = currentRoomNode.RuneRoomSaveData.RuneSelected;
    }

    private void Start()
    {
        if (currentRoomNode.RoomType != RoomType.Rune) return;
        
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

    public void SelectRune(RuneSocket runeSocket, Rune rune)
    {
        ActorCache.Instance.Player.RuneManager.AddRune(runeSocket, rune);
        RuneSelected = true;
        RuneSelectedSubject.Next();
        PersistenceManager.Instance.Save();
    }
}
