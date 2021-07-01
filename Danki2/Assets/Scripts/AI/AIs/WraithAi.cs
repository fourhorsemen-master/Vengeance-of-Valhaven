using UnityEngine;

public class WraithAi : Ai
{
    [SerializeField] private Wraith wraith = null;

    [Header("General")]
    [SerializeField] private float aggroRange = 0;
    [SerializeField] private float rangedAttackStateRange = 0;
    [SerializeField] private float rangedAttackStateTolerance = 0;
    
    [Header("Ranged Attacks")]
    [SerializeField] private float spineDelay = 0;
    [SerializeField] private float guidedOrbDelay = 0;
    
    [Header("Melee Attacks")]
    [SerializeField] private float swipeRotationSmoothingOverride;
    [SerializeField] private float swipeRange = 0;
    [SerializeField] private float maxSwipeAngle = 0;
    [SerializeField] private float swipeDelay = 0;
    
    [Header("Blink")]
    [SerializeField] private float forcedBlinkMinTime = 0;
    [SerializeField] private float forcedBlinkMaxTime = 0;
    [SerializeField] private int meleeAttacksBeforeBlinking = 0;
    [SerializeField] private float blinkDelay = 0;
    [SerializeField] private float postBlinkAttackDelay = 0;
    [SerializeField] private float minBlinkDistance = 0;
    [SerializeField] private float maxBlinkDistance = 0;
    
    protected override Actor Actor => wraith;
    
    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        Player player = ActorCache.Instance.Player;

        IStateMachineComponent rangedAttackStateMachine = new StateMachine<RangedAttackState>(RangedAttackState.ChooseAttack)
            .WithComponent(RangedAttackState.TelegraphSpine, new TelegraphAttackAndWatch(wraith, player, Color.green))
            .WithComponent(RangedAttackState.Spine, new WraithCastSpine(wraith, player))
            .WithComponent(RangedAttackState.TelegraphGuidedOrb, new TelegraphAttackAndWatch(wraith, player, Color.blue))
            .WithComponent(RangedAttackState.GuidedOrb, new WraithCastGuidedOrb(wraith, player))
            .WithDecisionState(RangedAttackState.ChooseAttack, new CyclicalDecider<RangedAttackState>(
                RangedAttackState.TelegraphSpine,
                RangedAttackState.TelegraphSpine,
                RangedAttackState.TelegraphGuidedOrb
            ))
            .WithTransition(RangedAttackState.TelegraphSpine, RangedAttackState.Spine, new CastableTimeElapsed(wraith, spineDelay))
            .WithTransition(RangedAttackState.Spine, RangedAttackState.ChooseAttack)
            .WithTransition(RangedAttackState.TelegraphGuidedOrb, RangedAttackState.GuidedOrb, new CastableTimeElapsed(wraith, guidedOrbDelay))
            .WithTransition(RangedAttackState.GuidedOrb, RangedAttackState.ChooseAttack);

        IStateMachineComponent meleeAttackStateMachine = new StateMachine<MeleeAttackState>(MeleeAttackState.WatchTarget)
            .WithComponent(MeleeAttackState.WatchTarget, new WatchTarget(wraith, player, swipeRotationSmoothingOverride))
            .WithComponent(MeleeAttackState.Telegraph, new TelegraphAttack(wraith, Color.yellow))
            .WithComponent(MeleeAttackState.Swipe, new WraithCastSwipe(wraith))
            .WithTransition(MeleeAttackState.WatchTarget, MeleeAttackState.Telegraph, new Facing(wraith, player, maxSwipeAngle))
            .WithTransition(MeleeAttackState.Telegraph, MeleeAttackState.Swipe, new CastableTimeElapsed(wraith, swipeDelay))
            .WithTransition(MeleeAttackState.Swipe, MeleeAttackState.WatchTarget);

        IStateMachineComponent blinkStateMachine = new StateMachine<BlinkState>(BlinkState.Telegraph)
            .WithComponent(BlinkState.Telegraph, new WraithTelegraphBlink(wraith))
            .WithComponent(BlinkState.Blink, new WraithCastBlink(wraith, player, minBlinkDistance, maxBlinkDistance))
            .WithComponent(BlinkState.PostBlinkAttack, new WraithCastSpine(wraith, player))
            .WithComponent(BlinkState.PostBlinkPause, new WatchTarget(wraith, player))
            .WithTransition(BlinkState.Telegraph, BlinkState.Blink, new CastableTimeElapsed(wraith, blinkDelay)) // TODO: Make blink castable through stun and knockback
            .WithTransition(BlinkState.Blink, BlinkState.PostBlinkAttack)
            .WithTransition(BlinkState.PostBlinkAttack, BlinkState.PostBlinkPause);

        return new StateMachine<State>(State.Idle)
            .WithComponent(State.Advance, new MoveTowards(wraith, player))
            .WithComponent(State.RangedAttacks, rangedAttackStateMachine)
            .WithComponent(State.MeleeAttacks, meleeAttackStateMachine)
            .WithComponent(State.Blink, blinkStateMachine)
            .WithTransition(State.Idle, State.Advance, new DistanceLessThan(wraith, player, aggroRange) | new TakesDamage(wraith))
            .WithTransition(State.Advance, State.RangedAttacks, new DistanceLessThan(wraith, player, rangedAttackStateRange))
            .WithTransition(State.RangedAttacks, State.Advance, new DistanceGreaterThan(wraith, player, rangedAttackStateRange + rangedAttackStateTolerance))
            .WithTransition(State.RangedAttacks, State.MeleeAttacks, new DistanceLessThan(player, wraith, swipeRange))
            .WithTransition(State.RangedAttacks, State.Blink, new RandomTimeElapsed(forcedBlinkMinTime, forcedBlinkMaxTime))
            .WithTransition(State.MeleeAttacks, State.Blink, new SubjectEmittedTimes(wraith.SwipeSubject, meleeAttacksBeforeBlinking))
            .WithTransition(State.Blink, State.Advance, new CastableTimeElapsed(wraith, blinkDelay + postBlinkAttackDelay));
    }

    private enum State
    {
        Idle,
        Advance,
        RangedAttacks,
        MeleeAttacks,
        Blink
    }

    private enum RangedAttackState
    {
        ChooseAttack,
        TelegraphSpine,
        Spine,
        TelegraphGuidedOrb,
        GuidedOrb
    }

    private enum MeleeAttackState
    {
        WatchTarget,
        Telegraph,
        Swipe
    }

    private enum BlinkState
    {
        Telegraph,
        Blink,
        PostBlinkAttack,
        PostBlinkPause
    }
}
