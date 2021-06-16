using UnityEngine;

public class ForestGolemAi : Ai
{
    [SerializeField] private ForestGolem forestGolem = null;

    [Header("General")]
    [SerializeField] private float aggroRange = 0;
    [SerializeField] private float advanceDistance = 0;
    [SerializeField] private float advanceDuration = 0;
    
    [Header("Root Storm")]
    [SerializeField] private float rootStormCooldown = 0;
    [SerializeField] private float minRootStormDistance = 0;
    [SerializeField] private float maxRootStormDistance = 0;
    [SerializeField] private float rootStormInterval = 0;
    [SerializeField] private float rootStormDuration = 0;

    [Header("Boulder Throw")]
    [SerializeField] private float boulderThrowTelegraphTime = 0;

    [Header("Stomp")]
    [SerializeField] private float stompRange = 0;
    [SerializeField] private float maxStompAngle = 0;
    [SerializeField] private float stompTelegraphTime = 0;

    protected override Actor Actor => forestGolem;
    
    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        Player player = ActorCache.Instance.Player;
        
        IStateMachineComponent rootStormStateMachine = new StateMachine<RootStormState>(RootStormState.Idle)
            .WithComponent(RootStormState.FireRoot, new ForestGolemFireRoot(forestGolem, minRootStormDistance, maxRootStormDistance))
            .WithTransition(RootStormState.Idle, RootStormState.FireRoot, new TimeElapsed(rootStormInterval))
            .WithTransition(RootStormState.FireRoot, RootStormState.Idle, new AlwaysTrigger());
        
        IStateMachineComponent boulderThrowStateMachine = new StateMachine<BoulderThrowState>(BoulderThrowState.Telegraph)
            .WithComponent(BoulderThrowState.Telegraph, new TelegraphAttack(forestGolem, Color.blue))
            .WithComponent(BoulderThrowState.ThrowBoulder, new ForestGolemThrowBoulder(forestGolem))
            .WithTransition(BoulderThrowState.Telegraph, BoulderThrowState.ThrowBoulder, new TimeElapsed(boulderThrowTelegraphTime));

        IStateMachineComponent stompStateMachine = new StateMachine<StompState>(StompState.WatchTarget)
            .WithComponent(StompState.WatchTarget, new WatchTarget(forestGolem, player))
            .WithComponent(StompState.Telegraph, new TelegraphAttack(forestGolem, Color.yellow))
            .WithComponent(StompState.Stomp, new ForestGolemStomp(forestGolem))
            .WithTransition(StompState.WatchTarget, StompState.Telegraph, new Facing(forestGolem, player, maxStompAngle))
            .WithTransition(StompState.Telegraph, StompState.Stomp, new TimeElapsed(stompTelegraphTime));
        
        return new StateMachine<State>(State.Idle)
            .WithComponent(State.Advance, new MoveTowardsAtDistance(forestGolem, player, advanceDistance))
            .WithComponent(State.RootStorm, rootStormStateMachine)
            .WithComponent(State.BoulderThrow, boulderThrowStateMachine)
            .WithComponent(State.Stomp, stompStateMachine)
            .WithDecisionState(State.DecideAttack, new ForestGolemAttackDecider(forestGolem, rootStormCooldown, stompRange))
            .WithTransition(State.Idle, State.Advance, new DistanceLessThan(forestGolem, player, aggroRange) | new TakesDamage(forestGolem))
            .WithTransition(State.Advance, State.DecideAttack, new TimeElapsed(advanceDuration))
            .WithTransition(State.RootStorm, State.DecideAttack, new TimeElapsed(rootStormDuration))
            .WithTransition(State.BoulderThrow, State.Advance, new SubjectEmitted(forestGolem.BoulderThrowSubject))
            .WithTransition(State.Stomp, State.Advance, new SubjectEmitted(forestGolem.StompSubject));
    }

    public enum State
    {
        Idle,
        Advance,
        DecideAttack,
        RootStorm,
        BoulderThrow,
        Stomp
    }

    private enum RootStormState
    {
        Idle,
        FireRoot
    }

    private enum BoulderThrowState
    {
        Telegraph,
        ThrowBoulder
    }

    private enum StompState
    {
        WatchTarget,
        Telegraph,
        Stomp
    }
}
