using System;

public interface Processor<TState> where TState : Enum
{
    void Enter();

    void Exit();

    bool TryCompleteProcess(out TState newState);
}