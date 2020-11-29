using System;

public class NoOpProcessor<TState> : Processor<TState> where TState : Enum
{
    public void Enter() { }

    public bool TryCompleteProcess(out TState newState) 
    {
        newState = default;
        return false;
    }

    public void Exit() { }
}