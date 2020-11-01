using UnityEngine;

public class WolfAi : Ai
{
    [SerializeField] private Wolf wolf = null;
    
    [Header("Overarching")]
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
    [SerializeField] private int minAttacks = 0;
    [SerializeField] private int maxAttacks = 0;
    [SerializeField] private float evadeTime = 0;
    [SerializeField] private float retreatTime = 0;
    [SerializeField] [Range(0, 1)] private float firstRetreatHealthProportion = 0;
    [SerializeField] [Range(0, 1)] private float secondRetreatHealthProportion = 0;

    [Header("Attack")]
    [SerializeField] private float followDistance = 0;
    [SerializeField] private float biteRange = 0;
    [SerializeField] private float biteCooldown = 0;
    [SerializeField] private float pounceMinRange = 0;
    [SerializeField] private float pounceMaxRange = 0;
    [SerializeField] private float pounceCooldown = 0;
    
    [Header("Evade")]
    [SerializeField] private float minCircleDistance = 0;
    [SerializeField] private float maxCircleDistance = 0;

    protected override Actor Actor => wolf;

    protected override IAiComponent BuildAiComponent()
    {
        Player player = RoomManager.Instance.Player;

        IAiComponent patrolStateMachine = new AiStateMachine<PatrolState>(PatrolState.StandStill)
            .WithComponent(PatrolState.StandStill, new StandStill(wolf))
            .WithComponent(PatrolState.RandomMovement, new MoveInRandomDirection(wolf))
            .WithTransition(PatrolState.StandStill, PatrolState.RandomMovement, new RandomTimeElapsed(minStillTime, maxStillTime))
            .WithTransition(PatrolState.RandomMovement, PatrolState.StandStill, new RandomTimeElapsed(minMovementTime, maxMovementTime));

        float circleDistance = (minCircleDistance + maxCircleDistance) / 2;

        IAiComponent attackStateMachine = new AiStateMachine<AttackState>(AttackState.Reposition)
            .WithComponent(AttackState.Reposition, new MoveTowardsAtDistance(wolf, player, followDistance))
            .WithComponent(AttackState.Bite, new WolfBite(wolf))
            .WithComponent(AttackState.Pounce, new WolfPounce(wolf, player))
            .WithTransition(
                AttackState.Reposition,
                AttackState.Bite,
                new AndTrigger(new DistanceLessThan(wolf, player, biteRange), new TimeElapsed(biteCooldown))
            )
            .WithTransition(AttackState.Bite, AttackState.Reposition, new WolfBiteFinished(wolf))
            .WithTransition(
                AttackState.Reposition,
                AttackState.Pounce,
                new AndTrigger(
                    new DistanceGreaterThan(wolf, player, pounceMinRange),
                    new DistanceLessThan(wolf, player, pounceMaxRange),
                    new TimeElapsed(pounceCooldown)
                )
            )
            .WithTransition(AttackState.Pounce, AttackState.Reposition, new WolfPounceFinished(wolf));

        IAiComponent evadeStateMachine = new AiStateMachine<EvadeState>(EvadeState.Circle)
            .WithComponent(EvadeState.Circle, new Circle(wolf, player))
            .WithComponent(EvadeState.MoveTowards, new MoveTowards(wolf, player))
            .WithComponent(EvadeState.MoveAway, new MoveAway(wolf, player))
            .WithTransition(EvadeState.MoveTowards, EvadeState.Circle, new DistanceLessThan(wolf, player, circleDistance))
            .WithTransition(EvadeState.MoveAway, EvadeState.Circle, new DistanceGreaterThan(wolf, player, circleDistance))
            .WithGlobalTransition(EvadeState.MoveTowards, new DistanceGreaterThan(wolf, player, maxCircleDistance))
            .WithGlobalTransition(EvadeState.MoveAway, new DistanceLessThan(wolf, player, minCircleDistance));

        IAiComponent engageStateMachine = new AiStateMachine<EngageState>(EngageState.Howl)
            .WithComponent(EngageState.Howl, new WolfHowl(wolf))
            .WithComponent(EngageState.Attack, attackStateMachine)
            .WithComponent(EngageState.Evade, evadeStateMachine)
            .WithComponent(EngageState.Retreat, new MoveAway(wolf, player))
            .WithTransition(EngageState.Howl, EngageState.Attack, new InstantTrigger())
            .WithTransition(EngageState.Attack, EngageState.Evade, new WolfRandomAttackCountReached(wolf, minAttacks, maxAttacks))
            .WithTransition(EngageState.Evade, EngageState.Attack, new TimeElapsed(evadeTime))
            .WithTransition(EngageState.Retreat, EngageState.Howl, new TimeElapsed(retreatTime))
            .WithGlobalTransition(
                EngageState.Retreat,
                new HealthGoesBelowProportion(wolf, firstRetreatHealthProportion),
                new HealthGoesBelowProportion(wolf, secondRetreatHealthProportion)
            );

        return new AiStateMachine<State>(State.Patrol)
            .WithComponent(State.Patrol, patrolStateMachine)
            .WithComponent(State.Watch, new WatchTarget(wolf, player))
            .WithComponent(State.Engage, engageStateMachine)
            .WithTransition(State.Patrol, State.Watch, new DistanceLessThan(wolf, player, watchDistance))
            .WithTransition(State.Watch, State.Patrol, new DistanceGreaterThan(wolf, player, watchDistance))
            .WithTransition(State.Watch, State.Engage, new TimeElapsed(agroTime), new DistanceLessThan(wolf, player, agroDistance))
            .WithGlobalTransition(State.Engage, new HearsHowl(wolf, howlHearingRange), new TakesDamage(wolf));
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
        Reposition,
        Bite,
        Pounce
    }

    private enum EvadeState
    {
        Circle,
        MoveTowards,
        MoveAway
    }

    private enum EngageState
    {
        Howl,
        Attack,
        Evade,
        Retreat
    }
}
