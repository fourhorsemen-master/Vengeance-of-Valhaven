public class HealingRoomManager : Singleton<HealingRoomManager>
{
    private const int HealingAmount = 5;
    
    private readonly Subject hasHealedSubject = new Subject();

    private RoomSaveData currentRoomSaveData;
    
    public bool HasHealed { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();

        currentRoomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        if (currentRoomSaveData.RoomType != RoomType.Healing) return;

        HasHealed = currentRoomSaveData.HealingRoomSaveData.HasHealed;
    }

    private void Start()
    {
        if (currentRoomSaveData.RoomType != RoomType.Healing) return;

        GameplayRoomTransitionManager.Instance.RegisterCanTransitionSubject(hasHealedSubject);
        
        if (HasHealed) hasHealedSubject.Next();
    }

    public void Heal()
    {
        if (HasHealed) return;

        ActorCache.Instance.Player.HealthManager.ReceiveHeal(HealingAmount);
        HasHealed = true;
        hasHealedSubject.Next();
        PersistenceManager.Instance.Save();
    }
}
