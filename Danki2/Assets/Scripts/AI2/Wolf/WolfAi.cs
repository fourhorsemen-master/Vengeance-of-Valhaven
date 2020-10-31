using UnityEngine;

public class WolfAi : Ai2
{
    [SerializeField] private Wolf wolf = null;

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
    [SerializeField] private int firstRetreatHealth = 0;
    [SerializeField] private int secondRetreatHealth = 0;
    
    [Header("Overarching")]
    [SerializeField] private float noticeDistance = 0;
    [SerializeField] private float noticeTime = 0;
    [SerializeField] private float engageDistance = 0;
    [SerializeField] private float howlHearingRange = 0;

    protected override Actor Actor => wolf;

    protected override IAiComponent BuildAiComponent()
    {
        IAiComponent patrolStateMachine = new AiStateMachine<PatrolState>(PatrolState.Still)
            .WithComponent(PatrolState.Still, new WolfStill())
            .WithComponent(PatrolState.RandomMovement, new WolfRandomMovement())
            .WithTransition(PatrolState.Still, PatrolState.RandomMovement, new IfRandomTimeElapsed(minStillTime, maxStillTime))
            .WithTransition(PatrolState.RandomMovement, PatrolState.Still, new IfRandomTimeElapsed(minMovementTime, maxMovementTime));

        IAiComponent engageStateMachine = new AiStateMachine<EngageState>(EngageState.Howl)
            .WithComponent(EngageState.Howl, new WolfHowl())
            .WithComponent(EngageState.Attack, new WolfAttack2())
            .WithComponent(EngageState.Evade, new WolfEvade())
            .WithComponent(EngageState.Retreat, new WolfRetreat())
            .WithTransition(EngageState.Howl, EngageState.Attack, new IfAnything())
            .WithTransition(EngageState.Attack, EngageState.Evade, new IfRandomWolfAttackCount(wolf, minAttacks, maxAttacks))
            .WithTransition(EngageState.Evade, EngageState.Attack, new IfTimeElapsed(evadeTime))
            .WithTransition(EngageState.Retreat, EngageState.Howl, new IfTimeElapsed(retreatTime))
            .WithGlobalTransition(EngageState.Retreat, new IfHealthGoesLessThan(wolf, firstRetreatHealth), new IfHealthGoesLessThan(wolf, secondRetreatHealth));

        Player player = RoomManager.Instance.Player;

        return new AiStateMachine<State>(State.Patrol)
            .WithComponent(State.Patrol, patrolStateMachine)
            .WithComponent(State.Notice, new WolfNotice())
            .WithComponent(State.Engage, engageStateMachine)
            .WithTransition(State.Patrol, State.Notice, new IfDistanceLessThan(wolf, player, noticeDistance))
            .WithTransition(State.Notice, State.Patrol, new IfDistanceGreaterThan(wolf, player, noticeDistance))
            .WithTransition(State.Notice, State.Engage, new IfTimeElapsed(noticeTime), new IfDistanceLessThan(wolf, player, engageDistance))
            .WithGlobalTransition(State.Engage, new IfHeardHowl(wolf, howlHearingRange), new IfTakenDamage(wolf));
    }

    private enum State
    {
        Patrol,
        Notice,
        Engage
    }

    private enum PatrolState
    {
        Still,
        RandomMovement
    }

    private enum EngageState
    {
        Howl,
        Attack,
        Evade,
        Retreat
    }
}
