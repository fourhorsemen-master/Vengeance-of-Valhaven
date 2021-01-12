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
    private MeshRenderer[] meshRenderers = null;

    // Serialized properties
    [SerializeField] private float weight = 0;
    public float Weight => weight;

    [SerializeField] private Transform centre = null;
    public Vector3 Centre => centre.position;

    [SerializeField] private Transform abilitySource = null;
    public Vector3 AbilitySource => abilitySource.position;

    protected readonly Subject startSubject = new Subject();
    protected readonly Subject updateSubject = new Subject();
    protected readonly Subject lateUpdateSubject = new Subject();

    private Coroutine stopTrailCoroutine;

    // Services
    public StatsManager StatsManager { get; private set; }
    public HealthManager HealthManager { get; private set; }
    public ChannelService ChannelService { get; private set; }
    public InstantCastService InstantCastService { get; private set; }
    public EffectManager EffectManager { get; private set; }
    public MovementManager MovementManager { get; private set; }
    public InterruptionManager InterruptionManager { get; private set; }
    public HighlightManager HightlightManager { get; private set; }

    public bool Dead { get; private set; }
    public Subject DeathSubject { get; } = new Subject();
    public abstract ActorType Type { get; }

    protected virtual void Awake()
    {
        StatsManager = new StatsManager(baseStats);
        EffectManager = new EffectManager(this, updateSubject);
        HealthManager = new HealthManager(this, updateSubject);
        InterruptionManager = new InterruptionManager(this, startSubject, updateSubject);
        ChannelService = new ChannelService(this, startSubject, lateUpdateSubject);
        InstantCastService = new InstantCastService(this);
        MovementManager = new MovementManager(this, updateSubject, navmeshAgent);
        HightlightManager = new HighlightManager(updateSubject, meshRenderers);

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
        
    public bool Opposes(Actor target)
    {
        return !CompareTag(target.tag);
    }

    public void InterruptibleAction(float delay, InterruptionType interruptionType, Action action)
    {
        Coroutine coroutine = this.WaitAndAct(delay, action);
        
        // We don't need to worry about deregistering the interruptible as Stopping a finished coroutine doesn't cause any problems.
        InterruptionManager.Register(
            interruptionType,
            () => StopCoroutine(coroutine),
            InterruptibleFeature.InterruptOnDeath
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
