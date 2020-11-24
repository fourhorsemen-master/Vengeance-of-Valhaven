using System;

public abstract class Processor<TState> where TState : Enum
{
    public abstract void Enter();

    public abstract void Exit();

    public abstract bool TryCompleteProcess(out TState newState);
}