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
        Player player = ActorCache.Instance.Player;
        
        // IStateMachineComponent rootStormStateMachine = new StateMachine<RootStormState>(RootStormState.Idle)
        //     .WithComponent(RootStormState.FireRoot, new ForestGolemFireRoot(forestGolem, minRootDistance, maxRootDistance))
        //     .WithTransition(RootStormState.Idle, RootStormState.FireRoot, new TimeElapsed(rootInterval))
        //     .WithTransition(RootStormState.FireRoot, RootStormState.Idle, new AlwaysTrigger());
        //
        // return new StateMachine<State>(State.RootStorm)
        //     .WithComponent(State.RootStorm, rootStormStateMachine)
        //     .WithTransition(State.Idle, State.RootStorm, new TimeElapsed(5))
        //     .WithTransition(State.RootStorm, State.Idle, new TimeElapsed(10));
        
        return new StateMachine<State>(State.Idle)
            .WithComponent(State.Idle, new WatchTarget(forestGolem, player))
            .WithComponent(State.ThrowBoulder, new ForestGolemThrowBoulder(forestGolem))
            .WithTransition(State.Idle, State.ThrowBoulder, new TimeElapsed(3))
            .WithTransition(State.ThrowBoulder, State.Idle, new AlwaysTrigger());
        
        // return new NoOpComponent();
    }

    private enum State
    {
        Idle,
        RootStorm,
        ThrowBoulder
    }

    private enum RootStormState
    {
        Idle,
        FireRoot
    }
}
