using UnityEngine;
using UnityEngine.AI;

public abstract class Actor : MonoBehaviour
{
    [HideInInspector]
    public StatsDictionary baseStats = new StatsDictionary(0);

    [SerializeField]
    private NavMeshAgent navmeshAgent = null;

    private StatsManager statsManager;
    private Subject updateSubject = new Subject();

    private float health;

    public ChannelService ChannelService { get; private set; }
    public EffectManager EffectManager { get; private set; }
    public MovementManager MovementManager { get; private set; }
    public Actor Target { get; set; } = null;
    public int Health => Mathf.CeilToInt(health);
    public bool Dead { get; private set; }
    public bool IsDamaged => Health < GetStat(Stat.MaxHealth);

    public abstract ActorType Type { get; }

    protected virtual void Awake()
    {
        this.statsManager = new StatsManager(baseStats);
        EffectManager = new EffectManager(this, updateSubject, statsManager);
        ChannelService = new ChannelService(updateSubject);
        MovementManager = new MovementManager(this, updateSubject, this.navmeshAgent);

        health = GetStat(Stat.MaxHealth);
        Dead = false;
    }

    protected virtual void Update()
    {
        if (health <= 0f && !Dead)
        {
            OnDeath();
            Dead = true;
        }

        if (Dead) return;

        this.updateSubject.Next();
    }

    public int GetStat(Stat stat)
    {
        return statsManager[stat];
    }

    public void ModifyHealth(float healthChange)
    {
        if (Dead) return;

        health = Mathf.Min(health + healthChange, GetStat(Stat.MaxHealth));
    }
        
    public bool Opposes(Actor target)
    {
        return tag != target.tag;
    }

    protected abstract void OnDeath();
}
