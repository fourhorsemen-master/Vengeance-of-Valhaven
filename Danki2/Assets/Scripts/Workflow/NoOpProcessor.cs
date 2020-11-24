using System;

public class NoOpProcessor<TState> : Processor<TState> where TState : Enum
{
    public override void Enter() { }

    public override bool TryCompleteProcess(out TState state) 
    {
        state = default;
        return false;
    }

    public override void Exit() { }
}