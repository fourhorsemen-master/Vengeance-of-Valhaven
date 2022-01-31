using UnityEngine;

public class EntAi : Ai
{
    [SerializeField] private Ent ent = null;

    [Header("Movement")]
    [SerializeField] private float minAdvanceDistance = 0;
    [SerializeField] private float minRangedDistance = 0;
    [SerializeField] private float maxRangedDistance = 0;

    [Header("Melee")]
    [SerializeField] private float aggroDistance = 0;
    [SerializeField] private float swipeMaxRange = 0;
    [SerializeField] private float swipeDelay = 0;
    [SerializeField] private float swipeCooldown = 0;
    [SerializeField] private float maxSwipeAngle = 0;

    [Header("Ranged")]
    [SerializeField] private float spineMinRange = 0;
    [SerializeField] private float spineDelay = 0;
    [SerializeField] private float spineCooldown = 0;

    protected override Actor Actor => ent;

    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        Player player = ActorCache.Instance.Player;

        return new StateMachine<State>(State.Idle)
            .WithComponent(State.Advance, new EntAdvance(ent, player, minAdvanceDistance, minRangedDistance, maxRangedDistance))
            .WithComponent(State.TelegraphSpine, new TelegraphAttackAndWatch(ent, player, Color.green))
            .WithComponent(State.TelegraphSwipe, new TelegraphAttack(ent, Color.red))
            .WithComponent(State.Spine, new EntCastSpine(ent, player))
            .WithComponent(State.Swipe, new EntCastSwipe(ent))
            .WithTransition(State.Idle, State.Advance, new TakesDamage(ent) | new DistanceLessThan(ent, player, aggroDistance))
            .WithTransition(State.TelegraphSpine, State.Spine, new TimeElapsed(spineDelay))
            .WithTransition(State.TelegraphSwipe, State.Swipe, new TimeElapsed(swipeDelay))
            .WithTransition(State.Spine, State.Advance)
            .WithTransition(State.Swipe, State.Advance)
            .WithTransition(
                State.Advance,
                State.TelegraphSpine,
                new DistanceGreaterThan(ent, player, spineMinRange) & new HasLineOfSight(ent.transform, player.transform) & new TimeElapsed(spineCooldown)
            )
            .WithTransition(
                State.Advance,
                State.TelegraphSwipe,
                new DistanceLessThan(ent, player, swipeMaxRange) & new TimeElapsed(swipeCooldown) & new Facing(ent, player, maxSwipeAngle)
            );
    }

    private enum State
    {
        Idle,
        Advance,
        TelegraphSpine,
        Spine,
        TelegraphSwipe,
        Swipe
    }
}
