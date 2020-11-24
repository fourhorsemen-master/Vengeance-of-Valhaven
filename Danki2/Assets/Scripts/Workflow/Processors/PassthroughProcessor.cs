using System;

public class PassthroughProcessor<TState> : Processor<TState> where TState : Enum
{
    private readonly TState nextState;

    public PassthroughProcessor(TState nextState)
    {
        this.nextState = nextState;
    }

    public override void Enter() { }

    public override bool TryCompleteProcess(out TState newState)
    {
        newState = nextState;
        return true;
    }

    public override void Exit() { }
}
