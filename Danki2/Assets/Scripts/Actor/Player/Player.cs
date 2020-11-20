using UnityEngine;

public class Player : Actor
{
    public override ActorType Type => ActorType.Player;
    
    [Header("Roll")]
    [SerializeField] private float totalRollCooldown = 1f;
    [SerializeField] private float rollDuration = 0.3f;
    [SerializeField] private float rollSpeedMultiplier = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource whiffAudio = null;
    [SerializeField] private AudioSource rollAudio = null;

    private float remainingRollCooldown = 0f;
    private Subscription<bool> abilityFeedbackSubscription;

    public Direction LastCastDirection { get; private set; }

    // Services
    public AbilityTree AbilityTree { get; private set; }
    public PlayerTargetFinder TargetFinder { get; private set; }
    
    // Subjects
    public Subject RollSubject { get; } = new Subject();
    public Subject<Direction> ChannelStartSubject { get; } = new Subject<Direction>();
    public Subject ComboCompleteSubject { get; } = new Subject();
    public Subject ComboFailedSubject { get; } = new Subject();
    public Subject ComboContinueSubject { get; } = new Subject();
    public Subject WhiffSubject { get; private set; }

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

    public void Cast(Direction direction)
    {
        if (!AbilityTree.CanWalkDirection(direction)) return;
        LastCastDirection = direction;

        AbilityReference abilityReference = AbilityTree.GetAbility(direction);

        AbilityType abilityType = AbilityLookup.Instance.GetAbilityType(abilityReference);

        if (abilityType == AbilityType.InstantCast)
        {
            InstantCastService.Cast(
                abilityReference,
                TargetFinder.TargetPosition,
                subject => SubscribeToFeedback(subject),
                TargetFinder.Target
            );
        }
        else if (abilityType == AbilityType.Channel)
        {
            ChannelStartSubject.Next(direction);
            ChannelService.StartChannel(
                abilityReference,
                subject => SubscribeToFeedback(subject)
            );
        }
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

    public void Whiff()
    {
        AbilityTree.Reset();
        PlayWhiffSound();
        WhiffSubject.Next();
    }

    private void PlayWhiffSound()
    {
        whiffAudio.Play();
    }

    private void TickRollCooldown()
    {
        remainingRollCooldown = Mathf.Max(0f, remainingRollCooldown - Time.deltaTime);
    }

    private void SubscribeToFeedback(Subject<bool> subject)
    {
        abilityFeedbackSubscription = subject.Subscribe(feedback =>
        {
            abilityFeedbackSubscription.Unsubscribe();
            AbilityTree.Walk(LastCastDirection);

            if (feedback == false) FailCombo();
            else if (AbilityTree.CanWalk()) ContinueCombo();
            else CompleteCombo();
        });
    }

    private void CompleteCombo()
    {
        AbilityTree.Reset();
        ComboCompleteSubject.Next();
    }

    private void ContinueCombo()
    {
        ComboContinueSubject.Next();
    }

    private void FailCombo()
    {
        AbilityTree.Reset();
        PlayWhiffSound();
        ComboFailedSubject.Next();
    }
}
