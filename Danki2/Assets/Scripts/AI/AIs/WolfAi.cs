using UnityEngine;

public class WolfAi : Ai
{
    [SerializeField] private Wolf wolf = null;
    
    [Header("General")]
    [SerializeField] private float watchDistance = 0;
    [SerializeField] private float agroTime = 0;
    [SerializeField] private float agroDistance = 0;
    [SerializeField] private float howlHearingRange = 0;

    [Header("Patrol")]
    [SerializeField] private float minStillTime = 0;
    [SerializeField] private float maxStillTime = 0;
    [SerializeField] private float minMovementTime = 0;
    [SerializeField] private float maxMovementTime = 0;

    [Header("Engage")]
    [SerializeField] private int minBites = 0;
    [SerializeField] private int maxBites = 0;
    [SerializeField] private float minEvadeTime = 0;
    [SerializeField] private float maxEvadeTime = 0;

    [Header("Attack")]
    [SerializeField] private float followDistance = 0;
    [SerializeField] private float biteRange = 0;
    [SerializeField] private float biteMaxAngle = 0;
    [SerializeField] private float biteDelay = 0;
    [SerializeField] private float biteCooldown = 0;
    [SerializeField] private float pounceMinRange = 0;
    [SerializeField] private float pounceMaxRange = 0;
    [SerializeField] private float pounceDelay = 0;
    [SerializeField] private float pounceCooldown = 0;
    
    [Header("Evade")]
    [SerializeField] private float minCircleDistance = 0;
    [SerializeField] private float maxCircleDistance = 0;

    protected override Actor Actor => wolf;

    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        Player player = ActorCache.Instance.Player;

        IStateMachineComponent patrolStateMachine = new StateMachine<PatrolState>(PatrolState.StandStill)
            .WithComponent(PatrolState.StandStill, new StandStill(wolf))
            .WithComponent(PatrolState.RandomMovement, new MoveInRandomDirection(wolf))
            .WithTransition(PatrolState.StandStill, PatrolState.RandomMovement, new RandomTimeElapsed(minStillTime, maxStillTime))
            .WithTransition(PatrolState.RandomMovement, PatrolState.StandStill, new RandomTimeElapsed(minMovementTime, maxMovementTime));

        IStateMachineComponent attackStateMachine = new StateMachine<AttackState>(AttackState.InitialReposition)
            .WithComponent(AttackState.InitialReposition, new MoveTowardsAtDistance(wolf, player, followDistance))
            .WithComponent(AttackState.Reposition, new MoveTowardsAtDistance(wolf, player, followDistance))
            .WithComponent(AttackState.TelegraphBite, new TelegraphAttack(wolf, Color.red))
            .WithComponent(AttackState.Bite, new WolfBite(wolf))
            .WithComponent(AttackState.TelegraphPounce, new TelegraphAttack(wolf, Color.green))
            .WithComponent(AttackState.Pounce, new WolfPounce(wolf, player))
            .WithTransition(
                AttackState.InitialReposition,
                AttackState.TelegraphBite,
                new DistanceLessThan(wolf, player, biteRange) & new Facing(wolf, player, biteMaxAngle)
            )
            .WithTransition(
                AttackState.InitialReposition,
                AttackState.TelegraphPounce,
                new DistanceWithin(wolf, player, pounceMinRange, pounceMaxRange)
            )
            .WithTransition(
                AttackState.Reposition,
                AttackState.TelegraphBite,
                new DistanceLessThan(wolf, player, biteRange) & new TimeElapsed(biteCooldown) & new Facing(wolf, player, biteMaxAngle)
            )
            .WithTransition(AttackState.TelegraphBite, AttackState.Reposition, new Interrupted(wolf, InterruptionType.Hard))
            .WithTransition(AttackState.TelegraphBite, AttackState.Bite, new TimeElapsed(biteDelay))
            .WithTransition(AttackState.Bite, AttackState.Reposition, new AlwaysTrigger())
            .WithTransition(
                AttackState.Reposition,
                AttackState.TelegraphPounce,
                new DistanceWithin(wolf, player, pounceMinRange, pounceMaxRange) & new TimeElapsed(pounceCooldown)
            )
            .WithTransition(AttackState.TelegraphPounce, AttackState.Reposition, new Interrupted(wolf, InterruptionType.Hard))
            .WithTransition(AttackState.TelegraphPounce, AttackState.Pounce, new TimeElapsed(pounceDelay))
            .WithTransition(AttackState.Pounce, AttackState.Reposition, new AlwaysTrigger());

        IStateMachineComponent evadeStateMachine = new StateMachine<EvadeState>(EvadeState.Circle)
            .WithComponent(EvadeState.Circle, new Circle(wolf, player))
            .WithComponent(EvadeState.MoveTowards, new MoveTowards(wolf, player))
            .WithTransition(EvadeState.MoveTowards, EvadeState.Circle, new DistanceLessThan(wolf, player, (minCircleDistance + maxCircleDistance) / 2))
            .WithTransition(EvadeState.Circle, EvadeState.MoveTowards, new DistanceGreaterThan(wolf, player, maxCircleDistance));

        IStateMachineComponent engageStateMachine = new StateMachine<EngageState>(EngageState.Howl)
            .WithComponent(EngageState.Howl, new WolfHowl(wolf))
            .WithComponent(EngageState.Attack, attackStateMachine)
            .WithComponent(EngageState.DashAway, new WolfDashAway(wolf, player))
            .WithComponent(EngageState.Evade, evadeStateMachine)
            .WithTransition(EngageState.Howl, EngageState.Attack, new AlwaysTrigger())
            .WithTransition(EngageState.Attack, EngageState.DashAway, new WolfRandomBiteCountReached(wolf, minBites, maxBites) & new CanMove(wolf))
            .WithTransition(EngageState.DashAway, EngageState.Evade, new TimeElapsed(wolf.DashDuration))
            .WithTransition(
                EngageState.Evade,
                EngageState.Attack,
                new RandomTimeElapsed(minEvadeTime, maxEvadeTime) | new DistanceLessThan(wolf, player, minCircleDistance) | new TakesNonTickDamage(wolf)
            );

        return new StateMachine<State>(State.Patrol)
            .WithComponent(State.Patrol, patrolStateMachine)
            .WithComponent(State.Watch, new WatchTarget(wolf, player))
            .WithComponent(State.Engage, engageStateMachine)
            .WithTransition(State.Patrol, State.Watch, new DistanceLessThan(wolf, player, watchDistance))
            .WithTransition(State.Watch, State.Patrol, new DistanceGreaterThan(wolf, player, watchDistance))
            .WithTransition(State.Watch, State.Engage, new TimeElapsed(agroTime) | new DistanceLessThan(wolf, player, agroDistance))
            .WithGlobalTransition(State.Engage, new HearsHowl(wolf, howlHearingRange) | new TakesDamage(wolf));
    }

    private enum State
    {
        Patrol,
        Watch,
        Engage
    }

    private enum PatrolState
    {
        StandStill,
        RandomMovement
    }

    private enum AttackState
    {
        InitialReposition,
        Reposition,
        TelegraphBite,
        Bite,
        TelegraphPounce,
        Pounce
    }

    private enum EvadeState
    {
        Circle,
        MoveTowards
    }

    private enum EngageState
    {
        Howl,
        Attack,
        DashAway,
        Evade
    }
}
