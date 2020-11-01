using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    public override ActorType Type => ActorType.Player;

    // Settings
    [HideInInspector]
    public float abilityCooldown = 1f;
    [HideInInspector]
    public float totalRollCooldown = 1f;
    [HideInInspector]
    public float rollDuration = 0.2f;
    [HideInInspector]
    public float rollSpeedMultiplier = 3f;
    [HideInInspector]
    public float abilityTimeoutLimit = 5f;
    private float remainingRollCooldown = 0f;

    // Components
    [SerializeField]
    private AudioSource whiffAudio = null;
    [SerializeField]
    private AudioSource rollAudio = null;

    // Services
    public AbilityTree AbilityTree { get; private set; }    
    public AbilityManager AbilityManager { get; private set; }
    public PlayerTargetFinder TargetFinder { get; private set; }
    
    // Subjects
    public Subject RollSubject { get; } = new Subject();

    private readonly ISet<AbilityReference> blockedAbilities = new HashSet<AbilityReference>
    {
        AbilityReference.Fireball,
        AbilityReference.Pounce,
        AbilityReference.Bite
    };


    protected override void Awake()
    {
        base.Awake();

        EnumDictionary<AbilityReference, int> ownedAbilities = new EnumDictionary<AbilityReference, int>(0);
        EnumUtils.ForEach<AbilityReference>(a =>
        {
            if (blockedAbilities.Contains(a)) return;
            ownedAbilities[a] = 10;
        });

        AbilityTree = AbilityTreeFactory.CreateTree(
            ownedAbilities,
            AbilityTreeFactory.CreateNode(AbilityReference.Slash),
            AbilityTreeFactory.CreateNode(AbilityReference.FanOfKnives)
        );

        RegisterAbilityDataDiffer(new AbilityDataOrbsDiffer(AbilityTree));
        SetAbilityBonusCalculator(new AbilityBonusOrbsCalculator(AbilityTree));
        AbilityManager = new AbilityManager(this, abilityTimeoutLimit, abilityCooldown, updateSubject, lateUpdateSubject);
        TargetFinder = new PlayerTargetFinder(this, updateSubject);
        new RandomAbilityManager(this);
    }

    protected override void Start()
    {
        base.Start();
        
        gameObject.tag = Tags.Player;
    }

    protected override void Update()
    {
        base.Update();

        TickRollCooldown();
    }

    public void Roll(Vector3 direction)
    {
        if (remainingRollCooldown > 0 || ChannelService.Active) return;

        bool rolled = MovementManager.TryLockMovement(
            MovementLockType.Dash,
            rollDuration,
            GetStat(Stat.Speed) * rollSpeedMultiplier,
            direction,
            direction
        );

        if (rolled)
        {
            remainingRollCooldown = totalRollCooldown;
            rollAudio.Play();
            RollSubject.Next();
            StartTrail(rollDuration * 2);
        }
    }

    public void PlayWhiffSound()
    {
        whiffAudio.Play();
    }

    private void TickRollCooldown()
    {
        remainingRollCooldown = Mathf.Max(0f, remainingRollCooldown - Time.deltaTime);
    }
}
