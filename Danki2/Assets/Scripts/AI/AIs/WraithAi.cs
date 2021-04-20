using UnityEngine;

public class WraithAi : Ai
{
    [SerializeField] private Wraith wraith = null;

    [Header("General")]
    [SerializeField] private float aggroDistance = 0;

    [Header("Engagement")]
    [SerializeField] private float attackDistance = 0;
    [SerializeField] private float advanceDistance = 0;

    protected override Actor Actor => wraith;
    
    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        Player player = ActorCache.Instance.Player;
        
        // IStateMachineComponent attackStateMachine = new NoOpComponent();
        //
        // IStateMachineComponent engageStateMachine = new StateMachine<EngageState>(EngageState.Advance)
        //     .WithComponent(EngageState.Advance, new MoveTowards(wraith, player))
        //     .WithComponent(EngageState.Attack, attackStateMachine)
        //     .WithComponent(EngageState.Blink, new WraithBlink(wraith))
        //     .WithTransition(EngageState.Advance, EngageState.Attack, new DistanceLessThan(wraith, player, attackDistance))
        //     .WithTransition(EngageState.Attack, EngageState.Advance, new DistanceGreaterThan(wraith, player, advanceDistance));
        //
        // return new StateMachine<State>(State.Idle)
        //     .WithComponent(State.Engage, engageStateMachine)
        //     .WithTransition(
        //         State.Idle,
        //         State.Engage,
        //         new DistanceLessThan(wraith, player, aggroDistance) | new TakesDamage(wraith)
        //     );
    }

    private enum State
    {
        Idle,
        Advance,
        RangedAttacks,
        Swipe,
        Blink
    }

    private enum RangeAttackState
    {
        Spine1,
        Spine2,
        GuidedOrb
    }
}
