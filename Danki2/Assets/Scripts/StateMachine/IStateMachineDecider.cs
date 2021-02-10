using System;

public interface IStateMachineDecider<TState> where TState : Enum
{
    TState Decide();
}