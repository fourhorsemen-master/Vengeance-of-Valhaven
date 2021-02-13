using UnityEngine;

public class BearAi : Ai
{
    [SerializeField] private Bear bear = null;

    [Header("General")]
    [SerializeField] private float aggroDistance = 0;
    [SerializeField, Tooltip("Value must be less than maxAttackRange.")] private float minAdvanceRange = 0;
    [SerializeField, Tooltip("Value must be greater than minAdvanceRange.")] private float maxAttackRange = 0;
    [SerializeField] private float maxChargeRange = 0;
    [SerializeField] private float chargeInterval = 0;

    [Header("Advance")]
    [SerializeField] private float minRunDistance = 0;

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
            .WithTransition(AdvanceState.Walk, AdvanceState.Run, new DistanceGreaterThan(bear, player, minRunDistance) | new TakesDamage(bear));

        IStateMachineComponent attackStateMachine = new StateMachine<AttackState>(AttackState.ChooseAbility)
            .WithComponent(AttackState.WatchTarget, new WatchTarget(bear, player))
            .WithComponent(AttackState.TelegraphSwipe, new TelegraphAttack(bear, Color.red))
            .WithComponent(AttackState.TelegraphMaul, new TelegraphAttack(bear, Color.green))
            .WithComponent(AttackState.TelegraphCleave, new TelegraphAttack(bear, Color.yellow))
            .WithComponent(AttackState.Swipe, new BearSwipe(bear))
            .WithComponent(AttackState.Maul, new BearMaul(bear))
            .WithComponent(AttackState.Cleave, new BearCleave(bear))
            .WithTransition(AttackState.WatchTarget, AttackState.ChooseAbility, new TimeElapsed(abilityInterval) & new Facing(bear, player, maxAttackAngle))
            .WithTransition(AttackState.TelegraphSwipe, AttackState.Swipe, new TimeElapsed(swipeDelay) & new CanCast(bear))
            .WithTransition(AttackState.TelegraphMaul, AttackState.Maul, new TimeElapsed(maulDelay) & new CanCast(bear))
            .WithTransition(AttackState.TelegraphCleave, AttackState.Cleave, new TimeElapsed(cleaveDelay) & new CanCast(bear))
            .WithTransition(AttackState.Swipe, AttackState.WatchTarget, new AlwaysTrigger())
            .WithTransition(AttackState.Maul, AttackState.WatchTarget, new AlwaysTrigger())
            .WithTransition(AttackState.Cleave, AttackState.WatchTarget, new AlwaysTrigger())
            .WithDecisionState(AttackState.ChooseAbility, new HealthBasedRandomSelection<AttackState>(
                bear,
                new AttackState[] { AttackState.TelegraphMaul, AttackState.TelegraphCleave },
                new AttackState[] { AttackState.TelegraphSwipe, AttackState.TelegraphMaul }
            ));

        return new StateMachine<State>(State.Idle)
            .WithComponent(State.Advance, advanceStateMachine)
            .WithComponent(State.Attack, attackStateMachine)
            .WithComponent(State.TelegraphCharge, new TelegraphAttack(bear, Color.blue))
            .WithComponent(State.Charge, new BearChannelCharge(bear, player))
            .WithTransition(State.Idle, State.Advance, new DistanceLessThan(bear, player, aggroDistance) | new TakesDamage(bear))
            .WithTransition(State.Advance, State.Attack, new DistanceLessThan(bear, player, minAdvanceRange))
            .WithTransition(State.Attack, State.Advance, new DistanceGreaterThan(bear, player, maxAttackRange) & !new IsTelegraphing(bear))
            .WithTransition(State.Advance, State.TelegraphCharge, new DistanceLessThan(bear, player, maxChargeRange) & new TimeElapsed(chargeInterval))
            .WithTransition(State.TelegraphCharge, State.Charge, new TimeElapsed(chargeDelay) & new CanCast(bear))
            .WithTransition(State.Charge, State.Advance, new ChannelComplete(bear));
    }

    private enum State
    {
        Idle,
        Advance,
        TelegraphCharge,
        Charge,
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
        TelegraphMaul,
        Maul,
        TelegraphCleave,
        Cleave
    }
}
