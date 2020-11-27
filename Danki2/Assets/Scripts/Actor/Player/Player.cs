﻿using UnityEngine;

public class Player : Actor
{
    public override ActorType Type => ActorType.Player;

    [Header("Ability tree")]
    [SerializeField] private float shortCooldown = 0.75f;
    [SerializeField] private float longCooldown = 1.5f;
    [SerializeField] private float comboTimeout = 2f;
    [SerializeField] private float feedbackTimeout = 1f;
    [SerializeField] private bool rollResetsCombo = false;

    [Header("Roll")]
    [SerializeField] private float totalRollCooldown = 1f;
    [SerializeField] private float rollDuration = 0.3f;
    [SerializeField] private float rollSpeedMultiplier = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource whiffAudio = null;
    [SerializeField] private AudioSource rollAudio = null;

    private bool readyToRoll = true;

    // Services
    public AbilityTree AbilityTree { get; private set; }
    public ComboManager ComboManager { get; private set; }
    public PlayerTargetFinder TargetFinder { get; private set; }
    
    // Subjects
    public Subject RollSubject { get; } = new Subject();
    public Subject<bool> AbilityFeedbackSubject { get; } = new Subject<bool>();
    public bool? FeedbackSinceLastCast { get; private set; } = null;

    protected override void Awake()
    {
        base.Awake();

        TargetFinder = new PlayerTargetFinder(this, updateSubject);

        ComboManager = new ComboManager(this, updateSubject, shortCooldown, longCooldown, comboTimeout, rollResetsCombo);

        InstantCastService.SetFeedbackTimeout(feedbackTimeout);
        ChannelService.SetFeedbackTimeout(feedbackTimeout);

        InstantCastService.FeedbackSubject.Subscribe(feedback => AbilityFeedbackSubject.Next(feedback));
        ChannelService.FeedbackSubject.Subscribe(feedback => AbilityFeedbackSubject.Next(feedback));

        AbilityFeedbackSubject.Subscribe(f => FeedbackSinceLastCast = f);
        ComboManager.SubscribeToStateExit(ComboState.ReadyAtRoot, () => FeedbackSinceLastCast = null);
        ComboManager.SubscribeToStateExit(ComboState.ReadyInCombo, () => FeedbackSinceLastCast = null);

        EnumDictionary<AbilityReference, int> ownedAbilities = new EnumDictionary<AbilityReference, int>(3);

        AbilityTree = AbilityTreeFactory.CreateTree(
            ownedAbilities,
            AbilityTreeFactory.CreateNode(AbilityReference.SweepingStrike),
            AbilityTreeFactory.CreateNode(AbilityReference.Lunge)
        );

        RegisterAbilityDataDiffer(new AbilityDataOrbsDiffer(AbilityTree));
        SetAbilityBonusCalculator(new AbilityBonusOrbsCalculator(AbilityTree));
    }

    protected override void Start()
    {
        base.Start();
        
        gameObject.tag = Tags.Player;
    }

    public void Roll(Vector3 direction)
    {
        if (!readyToRoll || ChannelService.Active) return;

        bool rolled = MovementManager.TryLockMovement(
            MovementLockType.Dash,
            rollDuration,
            GetStat(Stat.Speed) * rollSpeedMultiplier,
            direction,
            direction
        );

        if (rolled)
        {
            rollAudio.Play();
            RollSubject.Next();
            StartTrail(rollDuration * 2);

            readyToRoll = false;
            this.WaitAndAct(totalRollCooldown, () => readyToRoll = true);
        }
    }

    public void PlayWhiffSound()
    {
        whiffAudio.Play();
    }
}
