using UnityEngine;

public class WraithAi : Ai
{
    [SerializeField] private Wraith wraith = null;

    protected override Actor Actor => wraith;
    
    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        Player player = ActorCache.Instance.Player;

        IStateMachineComponent rangedAttackStateMachine = new StateMachine<RangedAttackState>(RangedAttackState.Decider)
            .WithComponent(RangedAttackState.Spine1, new WraithCastSpine(wraith, player))
            .WithComponent(RangedAttackState.Spine2, new WraithCastSpine(wraith, player))
            .WithComponent(RangedAttackState.GuidedOrb, new WraithCastGuidedOrb(wraith, player))
            .WithDecisionState(RangedAttackState.Decider, new UniformDecider<RangedAttackState>(
                RangedAttackState.Spine1,
                RangedAttackState.Spine2,
                RangedAttackState.GuidedOrb
            ))
            .WithTransition(RangedAttackState.Spine1, RangedAttackState.Spine2, new RandomTimeElapsed(2, 3))
            .WithTransition(RangedAttackState.Spine2, RangedAttackState.GuidedOrb, new RandomTimeElapsed(2, 3))
            .WithTransition(RangedAttackState.GuidedOrb, RangedAttackState.Spine1, new RandomTimeElapsed(2, 3));

        return new StateMachine<State>(State.Idle)
            .WithComponent(State.Advance, new MoveTowardsAtDistance(wraith, player, 8))
            .WithComponent(State.RangedAttacks, rangedAttackStateMachine)
            .WithComponent(State.Swipe, new WraithCastSwipe(wraith, player))
            .WithComponent(State.Blink, new WraithCastBlink(wraith, player, 5, 10))
            .WithTransition(State.Idle, State.Advance, new DistanceLessThan(wraith, player, 12) | new TakesDamage(wraith))
            .WithTransition(State.Advance, State.RangedAttacks, new DistanceLessThan(wraith, player, 10))
            .WithTransition(State.RangedAttacks, State.Advance, new DistanceGreaterThan(wraith, player, 12))
            .WithTransition(State.RangedAttacks, State.Swipe, new DistanceLessThan(player, wraith, 3))
            .WithTransition(State.RangedAttacks, State.Blink, new RandomTimeElapsed(10, 15))
            .WithTransition(State.Swipe, State.RangedAttacks, new DistanceGreaterThan(wraith, player, 4))
            .WithTransition(State.Swipe, State.Blink, new AlwaysTrigger())
            .WithTransition(State.Blink, State.Advance, new AlwaysTrigger());
    }

    private enum State
    {
        Idle,
        Advance,
        RangedAttacks,
        Swipe,
        Blink
    }

    private enum RangedAttackState
    {
        Decider,
        Spine1,
        Spine2,
        GuidedOrb
    }
}
