using System;
using UnityEngine;

public class Player : Actor
{
    public override ActorType Type => ActorType.Player;

    // Ability tree settings
    [HideInInspector]
    public float CooldownDuringCombo = 0.75f;
    [HideInInspector]
    public float CooldownAfterCombo = 1.5f;
    public float ComboTimeout = 2f;
    [HideInInspector]
    public float FeedbackTimeout = 1f;
    [HideInInspector]
    public bool RollResetsCombo = false;

    // Roll settings
    [HideInInspector]
    public float TotalRollCooldown = 1f;
    [HideInInspector]
    public float RollDuration = 0.3f;
    [HideInInspector]
    public float RollSpeedMultiplier = 2f;

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

    protected override void Awake()
    {
        base.Awake();

        EnumDictionary<AbilityReference, int> ownedAbilities = new EnumDictionary<AbilityReference, int>(3);

        AbilityTree = AbilityTreeFactory.CreateTree(
            ownedAbilities,
            AbilityTreeFactory.CreateNode(AbilityReference.SweepingStrike),
            AbilityTreeFactory.CreateNode(AbilityReference.Lunge)
        );

        RegisterAbilityDataDiffer(new AbilityDataOrbsDiffer(AbilityTree));
        SetAbilityBonusCalculator(new AbilityBonusOrbsCalculator(AbilityTree));
        AbilityManager = new AbilityManager(this, updateSubject, lateUpdateSubject);
        TargetFinder = new PlayerTargetFinder(this, updateSubject);
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
            RollDuration,
            GetStat(Stat.Speed) * RollSpeedMultiplier,
            direction,
            direction
        );

        if (rolled)
        {
            remainingRollCooldown = TotalRollCooldown;
            rollAudio.Play();
            RollSubject.Next();
            StartTrail(RollDuration * 2);
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
