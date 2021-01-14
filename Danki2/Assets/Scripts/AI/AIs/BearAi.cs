using UnityEngine;

public class BearAi : Ai
{
    [SerializeField] private Bear bear = null;

    [Header("General")]
    [SerializeField] private float aggroDistance = 0;
    [SerializeField] [Tooltip("Value must be less than maxAttackRange.")] private float minAdvanceRange = 0;
    [SerializeField] [Tooltip("Value must be greater than minAdvanceRange.")] private float maxAttackRange = 0;

    [Header("Advance")]
    [SerializeField] private float advanceMinTransitionTime = 0;
    [SerializeField] private float advanceMaxTransitionTime = 0;

    protected override Actor Actor => bear;

    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        Player player = RoomManager.Instance.Player;

        IStateMachineComponent advanceStateMachine = new StateMachine<AdvanceState>(AdvanceState.Walk)
            .WithComponent(AdvanceState.Walk, new WalkTowards(bear, player))
            .WithComponent(AdvanceState.Run, new MoveTowards(bear, player))
            .WithTransition(AdvanceState.Walk, AdvanceState.Run, new RandomTimeElapsed(advanceMinTransitionTime, advanceMaxTransitionTime) | new TakesDamage(bear))
            .WithTransition(AdvanceState.Run, AdvanceState.Walk, new RandomTimeElapsed(advanceMinTransitionTime, advanceMaxTransitionTime));

        IStateMachineComponent attackStateMachine = new WatchTarget(bear, player);

        return new StateMachine<State>(State.Idle)
            .WithComponent(State.Advance, advanceStateMachine)
            .WithComponent(State.Attack, attackStateMachine)
            .WithTransition(State.Idle, State.Advance, new DistanceLessThan(bear, player, aggroDistance) | new TakesDamage(bear))
            .WithTransition(State.Advance, State.Attack, new DistanceLessThan(bear, player, minAdvanceRange))
            .WithTransition(State.Attack, State.Advance, new DistanceGreaterThan(bear, player, maxAttackRange));
    }

    private enum State
    {
        Idle,
        Advance,
        Attack
    }

    private enum AdvanceState
    {
        Walk,
        Run
    }
}
