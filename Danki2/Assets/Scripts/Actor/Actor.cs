using System;
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

    public HealthManager HealthManager { get; private set; }
    public ChannelService ChannelService { get; private set; }
    public InstantCastService InstantCastService { get; private set; }
    public EffectManager EffectManager { get; private set; }
    public MovementManager MovementManager { get; private set; }
    public InterruptionManager InterruptionManager { get; private set; }
    public Actor Target { get; set; } = null;
    public bool IsDamaged => HealthManager.Health < HealthManager.MaxHealth;
    public bool Dead { get; private set; }

    public abstract ActorType Type { get; }

    protected virtual void Awake()
    {
        statsManager = new StatsManager(baseStats);
        EffectManager = new EffectManager(this, updateSubject, statsManager);
        HealthManager = new HealthManager(this, updateSubject);
        InterruptionManager = new InterruptionManager();
        MovementManager = new MovementManager(this, updateSubject, navmeshAgent);

        ChannelService = new ChannelService(this, lateUpdateSubject, InterruptionManager);
        InstantCastService = new InstantCastService(this);
        AbilityDataStatsDiffer abilityDataStatsDiffer = new AbilityDataStatsDiffer(this);
        RegisterAbilityDataDiffer(abilityDataStatsDiffer);

        Dead = false;
    }

    protected virtual void Update()
    {
        if (HealthManager.Health <= 0 && !Dead)
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
        lateUpdateSubject.Next();
    }

    public int GetStat(Stat stat)
    {
        return statsManager[stat];
    }
        
    public bool Opposes(Actor target)
    {
        return !CompareTag(target.tag);
    }

    public void DamageTarget(Actor target, int damage)
    {
        if (target.Dead) return;
        target.HealthManager.ReceiveDamage(EffectManager.ProcessOutgoingDamage(damage));
    }

    public void InterruptableAction(float delay, InterruptionType interruptionType, Action action)
    {
        Coroutine coroutine = this.WaitAndAct(delay, action);
        InterruptionManager.Register(interruptionType, () => StopCoroutine(coroutine));
    }

    protected abstract void OnDeath();

    protected void RegisterAbilityDataDiffer(IAbilityDataDiffer abilityDataDiffer)
    {
        ChannelService.RegisterAbilityDataDiffer(abilityDataDiffer);
        InstantCastService.RegisterAbilityDataDiffer(abilityDataDiffer);
    }

    protected void ReplaceAbilityBonusCalculator(IAbilityBonusCalculator abilityBonusCalculator)
    {
        ChannelService.ReplaceAbilityBonusCalculator(abilityBonusCalculator);
        InstantCastService.ReplaceAbilityBonusCalculator(abilityBonusCalculator);
    }
}
