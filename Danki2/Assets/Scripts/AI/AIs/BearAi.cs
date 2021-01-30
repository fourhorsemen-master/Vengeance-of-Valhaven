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

    [Header("Attack")]
    [SerializeField] private float swipeDelay = 0;
    [SerializeField] private float chargeDelay = 0;
    [SerializeField] private float maulDelay = 0;
    [SerializeField] private float cleaveDelay = 0;
    [SerializeField] private float abilityInterval = 0;
    [SerializeField] private float maxAttackAngle = 0;

    protected override Actor Actor => bear;

    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        Player player = RoomManager.Instance.Player;

        IStateMachineComponent advanceStateMachine = new StateMachine<AdvanceState>(AdvanceState.Walk)
            .WithComponent(AdvanceState.Walk, new WalkTowards(bear, player))
            .WithComponent(AdvanceState.Run, new MoveTowards(bear, player))
            .WithTransition(AdvanceState.Walk, AdvanceState.Run, new RandomTimeElapsed(advanceMinTransitionTime, advanceMaxTransitionTime) | new TakesDamage(bear))
            .WithTransition(AdvanceState.Run, AdvanceState.Walk, new RandomTimeElapsed(advanceMinTransitionTime, advanceMaxTransitionTime));

        IStateMachineComponent attackStateMachine = new StateMachine<AttackState>(AttackState.ChooseAbility)
            .WithComponent(AttackState.WatchTarget, new WatchTarget(bear, player))
            .WithComponent(AttackState.TelegraphSwipe, new TelegraphAttack(bear, swipeDelay))
            .WithComponent(AttackState.TelegraphCharge, new TelegraphAttack(bear, chargeDelay))
            .WithComponent(AttackState.TelegraphMaul, new TelegraphAttack(bear, maulDelay))
            .WithComponent(AttackState.TelegraphCleave, new TelegraphAttack(bear, cleaveDelay))
            .WithComponent(AttackState.Swipe, new BearSwipe(bear))
            .WithComponent(AttackState.Charge, new BearChannelCharge(bear))
            .WithComponent(AttackState.Maul, new BearMaul(bear))
            .WithComponent(AttackState.Cleave, new BearCleave(bear))
            .WithTransition(AttackState.WatchTarget, AttackState.ChooseAbility, new TimeElapsed(abilityInterval) & new Facing(bear, player, maxAttackAngle))
            .WithTransition(AttackState.TelegraphSwipe, AttackState.Swipe, new TimeElapsed(swipeDelay))
            .WithTransition(AttackState.TelegraphSwipe, AttackState.WatchTarget, new Interrupted(bear, InterruptionType.Hard))
            .WithTransition(AttackState.TelegraphCharge, AttackState.Charge, new TimeElapsed(chargeDelay))
            .WithTransition(AttackState.TelegraphCharge, AttackState.WatchTarget, new Interrupted(bear, InterruptionType.Hard))
            .WithTransition(AttackState.TelegraphMaul, AttackState.Maul, new TimeElapsed(maulDelay))
            .WithTransition(AttackState.TelegraphMaul, AttackState.WatchTarget, new Interrupted(bear, InterruptionType.Hard))
            .WithTransition(AttackState.TelegraphCleave, AttackState.Cleave, new TimeElapsed(cleaveDelay))
            .WithTransition(AttackState.TelegraphCleave, AttackState.WatchTarget, new Interrupted(bear, InterruptionType.Hard))
            .WithTransition(AttackState.Swipe, AttackState.WatchTarget, new AlwaysTrigger())
            .WithTransition(AttackState.Charge, AttackState.WatchTarget, new ChannelComplete(bear))
            .WithTransition(AttackState.Maul, AttackState.WatchTarget, new AlwaysTrigger())
            .WithTransition(AttackState.Cleave, AttackState.WatchTarget, new AlwaysTrigger())
            .WithRandomTransition(AttackState.ChooseAbility, AttackState.TelegraphSwipe, AttackState.TelegraphCharge, AttackState.TelegraphMaul, AttackState.TelegraphCleave);

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

    private enum AttackState
    {
        WatchTarget,
        ChooseAbility,
        TelegraphSwipe,
        Swipe,
        TelegraphCharge,
        Charge,
        TelegraphMaul,
        Maul,
        TelegraphCleave,
        Cleave
    }
}
