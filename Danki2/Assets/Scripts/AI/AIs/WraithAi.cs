using UnityEngine;

public class WraithAi : Ai
{
    [SerializeField] private Wraith wraith = null;

    protected override Actor Actor => wraith;
    
    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        return new StateMachine<State>(State.Follow)
            .WithComponent(State.Follow, new MoveTowards(wraith, ActorCache.Instance.Player))
            .WithComponent(State.Blink, new WraithBlink(wraith))
            .WithTransition(State.Follow, State.Blink, new ButtonDown("Interact"))
            .WithTransition(State.Blink, State.Follow, new AlwaysTrigger());
    }

    private enum State
    {
        Follow,
        Blink
    }
}
