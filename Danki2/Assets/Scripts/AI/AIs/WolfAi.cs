﻿using UnityEngine;

public class WolfAi : Ai
{
    [SerializeField] private Wolf wolf = null;
    
    [Header("Overarching")]
    [SerializeField] private float noticeDistance = 0;
    [SerializeField] private float noticeTime = 0;
    [SerializeField] private float engageDistance = 0;
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
    [SerializeField] private int firstRetreatHealth = 0;
    [SerializeField] private int secondRetreatHealth = 0;

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
            .WithTransition(PatrolState.StandStill, PatrolState.RandomMovement, new IfRandomTimeElapsed(minStillTime, maxStillTime))
            .WithTransition(PatrolState.RandomMovement, PatrolState.StandStill, new IfRandomTimeElapsed(minMovementTime, maxMovementTime));

        float circleDistance = (minCircleDistance + maxCircleDistance) / 2;

        IAiComponent attackStateMachine = new AiStateMachine<AttackState>(AttackState.Reposition)
            .WithComponent(AttackState.Reposition, new MoveTowardsAtDistance(wolf, player, followDistance))
            .WithComponent(AttackState.Bite, new WolfBite(wolf))
            .WithComponent(AttackState.Pounce, new WolfPounce(wolf, player))
            .WithTransition(
                AttackState.Reposition,
                AttackState.Bite,
                new IfAllTrigger(new IfDistanceLessThan(wolf, player, biteRange), new IfTimeElapsed(biteCooldown))
            )
            .WithTransition(AttackState.Bite, AttackState.Reposition, new IfWolfBiteDone(wolf))
            .WithTransition(
                AttackState.Reposition,
                AttackState.Pounce,
                new IfAllTrigger(
                    new IfDistanceGreaterThan(wolf, player, pounceMinRange),
                    new IfDistanceLessThan(wolf, player, pounceMaxRange),
                    new IfTimeElapsed(pounceCooldown))
            )
            .WithTransition(AttackState.Pounce, AttackState.Reposition, new IfWolfPounceDone(wolf));

        IAiComponent evadeStateMachine = new AiStateMachine<EvadeState>(EvadeState.Circle)
            .WithComponent(EvadeState.Circle, new Circle(wolf, player))
            .WithComponent(EvadeState.MoveTowards, new MoveTowards(wolf, player))
            .WithComponent(EvadeState.MoveAway, new MoveAway(wolf, player))
            .WithTransition(EvadeState.MoveTowards, EvadeState.Circle, new IfDistanceLessThan(wolf, player, circleDistance))
            .WithTransition(EvadeState.MoveAway, EvadeState.Circle, new IfDistanceGreaterThan(wolf, player, circleDistance))
            .WithGlobalTransition(EvadeState.MoveTowards, new IfDistanceGreaterThan(wolf, player, maxCircleDistance))
            .WithGlobalTransition(EvadeState.MoveAway, new IfDistanceLessThan(wolf, player, minCircleDistance));

        IAiComponent engageStateMachine = new AiStateMachine<EngageState>(EngageState.Howl)
            .WithComponent(EngageState.Howl, new WolfHowl(wolf))
            .WithComponent(EngageState.Attack, attackStateMachine)
            .WithComponent(EngageState.Evade, evadeStateMachine)
            .WithComponent(EngageState.Retreat, new MoveAway(wolf, player))
            .WithTransition(EngageState.Howl, EngageState.Attack, new IfAnything())
            .WithTransition(EngageState.Attack, EngageState.Evade, new IfRandomWolfAttackCount(wolf, minAttacks, maxAttacks))
            .WithTransition(EngageState.Evade, EngageState.Attack, new IfTimeElapsed(evadeTime))
            .WithTransition(EngageState.Retreat, EngageState.Howl, new IfTimeElapsed(retreatTime))
            .WithGlobalTransition(EngageState.Retreat, new IfHealthGoesBelow(wolf, firstRetreatHealth), new IfHealthGoesBelow(wolf, secondRetreatHealth));

        return new AiStateMachine<State>(State.Patrol)
            .WithComponent(State.Patrol, patrolStateMachine)
            .WithComponent(State.Watch, new WatchTarget(wolf, player))
            .WithComponent(State.Engage, engageStateMachine)
            .WithTransition(State.Patrol, State.Watch, new IfDistanceLessThan(wolf, player, noticeDistance))
            .WithTransition(State.Watch, State.Patrol, new IfDistanceGreaterThan(wolf, player, noticeDistance))
            .WithTransition(State.Watch, State.Engage, new IfTimeElapsed(noticeTime), new IfDistanceLessThan(wolf, player, engageDistance))
            .WithGlobalTransition(State.Engage, new IfHeardHowl(wolf, howlHearingRange), new IfTakenDamage(wolf));
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
