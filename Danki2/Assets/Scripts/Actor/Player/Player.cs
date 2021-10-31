﻿using FMODUnity;
using UnityEngine;

public class Player : Actor
{
    public override ActorType Type => ActorType.Player;

    [Header("Ability tree")]
    [SerializeField] private float shortCooldown = 0.75f;
    [SerializeField] private float longCooldown = 1.5f;
    [SerializeField] private float comboTimeout = 2f;
    [SerializeField] private bool selfEmpoweringAbilities = false;

    [Header("Roll")]
    [SerializeField] private float totalRollCooldown = 1f;
    [SerializeField] private float rollDuration = 0.3f;
    [SerializeField] private float rollSpeedMultiplier = 2f;

    [Header("Fmod events")]
    [EventRef] [SerializeField] private string vocalisationEvent = null;

    [SerializeField]
    private AbilityAnimator abilityAnimator = null;

    [SerializeField]
    private AbilityAnimationListener abilityAnimationListener = null;
    public AbilityAnimationListener AbilityAnimationListener => abilityAnimationListener;

    private bool readyToRoll = true;

    public float ShortCooldown => shortCooldown;
    public float LongCooldown => longCooldown;
    public float ComboTimeout => comboTimeout;

    public bool CanCast => !Dead && !MovementManager.Stunned && !MovementManager.MovementLocked;

    // Services
    public AbilityTree AbilityTree { get; private set; }
    public ComboManager ComboManager { get; private set; }
    public PlayerTargetFinder TargetFinder { get; private set; }
    public RuneManager RuneManager { get; private set; }
    public CurrencyManager CurrencyManager { get; private set; }
    public AbilityService AbilityService { get; private set; }
    public PlayerMovementManager MovementManager { get; private set; }
    
    // Subjects
    public Subject RollSubject { get; } = new Subject();

    public string VocalisationEvent { get => vocalisationEvent; }

    protected override Tag Tag => Tag.Player;

    protected override void Awake()
    {
        base.Awake();

        AbilityTree = PersistenceManager.Instance.SaveData.SerializableAbilityTree.Deserialize();
        ComboManager = new ComboManager(this, updateSubject);
        TargetFinder = new PlayerTargetFinder(this, updateSubject);
        RuneManager = new RuneManager(this);
        CurrencyManager = new CurrencyManager();
        AbilityService = new AbilityService(this, abilityAnimator, selfEmpoweringAbilities);
        MovementManager = new PlayerMovementManager(this, updateSubject, navmeshAgent);
    }

    public void Roll(Vector3 direction)
    {
        if (!readyToRoll) return;

        bool rolled = MovementManager.TryLockMovement(
            MovementLockType.Roll,
            rollDuration,
            StatsManager.Get(Stat.Speed) * rollSpeedMultiplier,
            direction,
            direction
        );

        if (rolled)
        {
            // FMOD_TODO: play roll event here
            RollSubject.Next();
            StartTrail(rollDuration * 2);

            readyToRoll = false;
            this.WaitAndAct(totalRollCooldown, () => readyToRoll = true);

            AnimController.Play("Dash_OneShot");
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        PersistenceManager.Instance.TransitionToDefeatRoom();
    }
}
