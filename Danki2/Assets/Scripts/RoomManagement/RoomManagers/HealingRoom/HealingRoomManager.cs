public class HealingRoomManager : Singleton<HealingRoomManager>
{
    private const int HealingAmount = 5;
    
    private readonly Subject hasHealedSubject = new Subject();
    
    public bool HasHealed { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();

        RoomSaveData roomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;
        if (roomSaveData.RoomType != RoomType.Healing) return;

        HasHealed = roomSaveData.HealingRoomSaveData.HasHealed;
    }

    private void Start()
    {
        if (PersistenceManager.Instance.SaveData.CurrentRoomSaveData.RoomType != RoomType.Healing) return;

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
