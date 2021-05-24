using UnityEngine;

public class ForestGolemAi : Ai
{
    [SerializeField] private ForestGolem forestGolem = null;

    [Header("Root Storm")]
    [SerializeField] private float minRootDistance = 0;
    [SerializeField] private float maxRootDistance = 0;
    [SerializeField] private float rootInterval = 0;

    protected override Actor Actor => forestGolem;
    
    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        IStateMachineComponent rootStormStateMachine = new StateMachine<RootStormState>(RootStormState.Idle)
            .WithComponent(RootStormState.FireRoot, new ForestGolemFireRoot(forestGolem, minRootDistance, maxRootDistance))
            .WithTransition(RootStormState.Idle, RootStormState.FireRoot, new TimeElapsed(rootInterval))
            .WithTransition(RootStormState.FireRoot, RootStormState.Idle, new AlwaysTrigger());

        return new StateMachine<State>(State.Idle)
            .WithComponent(State.RootStorm, rootStormStateMachine)
            .WithTransition(State.Idle, State.RootStorm, new TimeElapsed(5))
            .WithTransition(State.RootStorm, State.Idle, new TimeElapsed(10));
    }

    private enum State
    {
        Idle,
        RootStorm
    }

    private enum RootStormState
    {
        Idle,
        FireRoot
    }
}
