using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class Actor : MonoBehaviour
{
    [HideInInspector]
    public StatsDictionary baseStats = new StatsDictionary(0);

    [SerializeField]
    private NavMeshAgent navmeshAgent = null;

    [SerializeField]
    private TrailRenderer trailRenderer = null;

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    private Coroutine stopTrailCoroutine;
    private StatsManager statsManager;

    protected readonly Subject startSubject = new Subject();
    protected readonly Subject updateSubject = new Subject();
    protected readonly Subject lateUpdateSubject = new Subject();

    public HealthManager HealthManager { get; private set; }
    public ChannelService ChannelService { get; private set; }
    public InstantCastService InstantCastService { get; private set; }
    public EffectManager EffectManager { get; private set; }
    public MovementManager MovementManager { get; private set; }
    public InterruptionManager InterruptionManager { get; private set; }
    public HighlightManager HightlightManager { get; private set; }

    public bool Dead { get; private set; }
    public Subject DeathSubject { get; } = new Subject();

    public virtual Vector3 Centre => transform.position + Vector3.up * MouseGamePositionFinder.Instance.HeightOffset;

    public abstract ActorType Type { get; }

    protected virtual void Awake()
    {
        statsManager = new StatsManager(baseStats);
        EffectManager = new EffectManager(this, updateSubject, statsManager);
        HealthManager = new HealthManager(this, updateSubject);
        InterruptionManager = new InterruptionManager(this, startSubject);
        ChannelService = new ChannelService(this, startSubject, lateUpdateSubject);
        InstantCastService = new InstantCastService(this);
        MovementManager = new MovementManager(this, updateSubject, navmeshAgent);
        HightlightManager = new HighlightManager(updateSubject, meshRenderer);

        AbilityDataStatsDiffer abilityDataStatsDiffer = new AbilityDataStatsDiffer(this);
        RegisterAbilityDataDiffer(abilityDataStatsDiffer);

        Dead = false;
    }

    protected virtual void Start()
    {
        startSubject.Next();

        gameObject.layer = Layers.Actors;
    }

    protected virtual void Update()
    {
        updateSubject.Next();
    }

    protected virtual void LateUpdate()
    {
        lateUpdateSubject.Next();

        if (HealthManager.Health <= 0 && !Dead)
        {
            OnDeath();
        }
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
        target.HealthManager.ReceiveDamage(EffectManager.ProcessOutgoingDamage(damage), this);
    }

    public void InterruptableAction(float delay, InterruptionType interruptionType, Action action)
    {
        Coroutine coroutine = this.WaitAndAct(delay, action);
        
        // We don't need to worry about deregistering the interruptable as Stopping a finished coroutine doesn't cause any problems.
        InterruptionManager.Register(
            interruptionType,
            () => StopCoroutine(coroutine),
            InterruptableFeature.InterruptOnDeath
        );
    }

    public void StartTrail(float duration)
    {
        trailRenderer.emitting = true;

        if (stopTrailCoroutine != null)
        {
            StopCoroutine(stopTrailCoroutine);
        }

        stopTrailCoroutine = this.WaitAndAct(duration, () => trailRenderer.emitting = false);
    }

    protected virtual void OnDeath()
    {
        Debug.Log($"{tag} died");

        DeathSubject.Next();
        Dead = true;
    }

    protected void RegisterAbilityDataDiffer(IAbilityDataDiffer abilityDataDiffer)
    {
        ChannelService.RegisterAbilityDataDiffer(abilityDataDiffer);
        InstantCastService.RegisterAbilityDataDiffer(abilityDataDiffer);
    }

    protected void SetAbilityBonusCalculator(IAbilityBonusCalculator abilityBonusCalculator)
    {
        ChannelService.SetAbilityBonusCalculator(abilityBonusCalculator);
        InstantCastService.SetAbilityBonusCalculator(abilityBonusCalculator);
    }
}
