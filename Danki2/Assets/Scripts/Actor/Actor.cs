using UnityEngine;
using UnityEngine.AI;

public abstract class Actor : MonoBehaviour
{
    [HideInInspector]
    public StatsDictionary baseStats = new StatsDictionary(0);

    [SerializeField]
    private NavMeshAgent navmeshAgent = null;

    private StatsManager statsManager;
    protected readonly Subject updateSubject = new Subject();
    protected readonly Subject lateUpdateSubject = new Subject();

    public ChannelService ChannelService { get; private set; }
    public EffectManager EffectManager { get; private set; }
    public MovementManager MovementManager { get; private set; }
    public InterruptionManager InterruptionManager { get; private set; }
    public Actor Target { get; set; } = null;
    public int Health { get; private set; }
    public bool Dead { get; private set; }
    public bool IsDamaged => Health < GetStat(Stat.MaxHealth);

    public abstract ActorType Type { get; }

    protected virtual void Awake()
    {
        statsManager = new StatsManager(baseStats);
        EffectManager = new EffectManager(this, updateSubject, statsManager);
        InterruptionManager = new InterruptionManager();
        ChannelService = new ChannelService(updateSubject, InterruptionManager);
        MovementManager = new MovementManager(this, updateSubject, navmeshAgent);

        Health = GetStat(Stat.MaxHealth);
        Dead = false;
    }

    protected virtual void Update()
    {
        if (Health <= 0 && !Dead)
        {
            MovementManager.StopPathfinding();
            OnDeath();
            Dead = true;
        }

        if (Dead) return;

        updateSubject.Next();
    }

    protected virtual void LateUpdate()
    {
        this.lateUpdateSubject.Next();
    }

    public int GetStat(Stat stat)
    {
        return statsManager[stat];
    }

    public void ModifyHealth(int healthChange)
    {
        if (Dead) return;

        Health = Mathf.Min(Health + healthChange, GetStat(Stat.MaxHealth));
    }
        
    public bool Opposes(Actor target)
    {
        return tag != target.tag;
    }

    protected abstract void OnDeath();
}
