using System;

public class PassthroughProcessor<TState> : Processor<TState> where TState : Enum
{
    private readonly TState nextState;

    public PassthroughProcessor(TState nextState)
    {
        this.nextState = nextState;
    }

    public void Enter() { }

    public bool TryCompleteProcess(out TState newState)
    {
        newState = nextState;
        return true;
    }

    public void Exit() { }
}
