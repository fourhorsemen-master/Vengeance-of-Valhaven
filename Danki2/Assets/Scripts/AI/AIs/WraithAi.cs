using UnityEngine;

public class WraithAi : Ai
{
    [SerializeField] private Wraith wraith = null;

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
            .WithTransition(RangedAttackState.TelegraphSpine, RangedAttackState.Spine, new TimeElapsed(2))
            .WithTransition(RangedAttackState.Spine, RangedAttackState.ChooseAttack)
            .WithTransition(RangedAttackState.TelegraphGuidedOrb, RangedAttackState.GuidedOrb, new TimeElapsed(2))
            .WithTransition(RangedAttackState.GuidedOrb, RangedAttackState.ChooseAttack);

        IStateMachineComponent meleeAttackStateMachine = new StateMachine<MeleeAttackState>(MeleeAttackState.Advance)
            .WithComponent(MeleeAttackState.Advance, new MoveTowards(wraith, player))
            .WithComponent(MeleeAttackState.WatchTarget, new WatchTarget(wraith, player))
            .WithComponent(MeleeAttackState.Telegraph, new TelegraphAttack(wraith, Color.yellow))
            .WithComponent(MeleeAttackState.Swipe, new WraithCastSwipe(wraith))
            .WithTransition(MeleeAttackState.Advance, MeleeAttackState.WatchTarget, new DistanceLessThan(wraith, player, 2))
            .WithTransition(MeleeAttackState.WatchTarget, MeleeAttackState.Telegraph, new Facing(wraith, player, 10))
            .WithTransition(MeleeAttackState.Telegraph, MeleeAttackState.Swipe, new TimeElapsed(0.5f))
            .WithTransition(MeleeAttackState.Swipe, MeleeAttackState.Advance);

        IStateMachineComponent blinkStateMachine = new StateMachine<BlinkState>(BlinkState.Telegraph)
            .WithComponent(BlinkState.Telegraph, new TelegraphAttack(wraith, Color.cyan))
            .WithComponent(BlinkState.Blink, new WraithCastBlink(wraith, player, 5, 10))
            .WithComponent(BlinkState.PostBlinkPause, new WatchTarget(wraith, player))
            .WithTransition(BlinkState.Telegraph, BlinkState.Blink, new TimeElapsed(2))
            .WithTransition(BlinkState.Blink, BlinkState.PostBlinkPause);

        return new StateMachine<State>(State.Idle)
            .WithComponent(State.Advance, new MoveTowardsAtDistance(wraith, player, 8))
            .WithComponent(State.RangedAttacks, rangedAttackStateMachine)
            .WithComponent(State.MeleeAttacks, meleeAttackStateMachine)
            .WithComponent(State.Blink, blinkStateMachine)
            .WithTransition(State.Idle, State.Advance, new DistanceLessThan(wraith, player, 12) | new TakesDamage(wraith))
            .WithTransition(State.Advance, State.RangedAttacks, new DistanceLessThan(wraith, player, 10))
            .WithTransition(State.RangedAttacks, State.Advance, new DistanceGreaterThan(wraith, player, 12))
            .WithTransition(State.RangedAttacks, State.MeleeAttacks, new DistanceLessThan(player, wraith, 3))
            .WithTransition(State.RangedAttacks, State.Blink, new RandomTimeElapsed(10, 15))
            .WithTransition(State.MeleeAttacks, State.RangedAttacks, new DistanceGreaterThan(wraith, player, 4))
            .WithTransition(State.MeleeAttacks, State.Blink, new SubjectEmittedTimes(wraith.SwipeSubject, 2))
            .WithTransition(State.Blink, State.Advance, new TimeElapsed(2));
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
        Advance,
        WatchTarget,
        Telegraph,
        Swipe
    }

    private enum BlinkState
    {
        Telegraph,
        Blink,
        PostBlinkPause
    }
}
