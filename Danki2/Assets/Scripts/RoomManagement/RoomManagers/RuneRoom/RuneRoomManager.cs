public class RuneRoomManager : Singleton<RuneRoomManager>
{
    public bool HasIncrementedRuneIndex { get; private set; } = false;
    public bool RunesViewed { get; private set; } = false;
    public bool RuneSelected { get; private set; } = false;

    private readonly Subject runesViewedSubject = new Subject();

    public Subject RuneSelectedSubject { get; } = new Subject();

    protected override void Awake()
    {
        base.Awake();

        RoomSaveData roomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        if (roomSaveData.RoomType != RoomType.Rune) return;

        HasIncrementedRuneIndex = roomSaveData.RuneRoomSaveData.HasIncrementedRuneIndex;
        RunesViewed = roomSaveData.RuneRoomSaveData.RunesViewed;
        RuneSelected = roomSaveData.RuneRoomSaveData.RuneSelected;
    }

    private void Start()
    {
        if (PersistenceManager.Instance.SaveData.CurrentRoomSaveData.RoomType != RoomType.Rune) return;

        if (!HasIncrementedRuneIndex)
        {
            ActorCache.Instance.Player.RuneManager.IncrementRuneIndex();
            HasIncrementedRuneIndex = true;
        }
        
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
