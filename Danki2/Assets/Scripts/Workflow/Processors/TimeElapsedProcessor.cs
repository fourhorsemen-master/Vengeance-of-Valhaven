using System;
using UnityEngine;

public class TimeElapsedProcessor<TState> : Processor<TState> where TState : Enum
{
    private readonly TState nextState;
    private readonly float duration;
    private float remainingDuration;

    public TimeElapsedProcessor(TState nextState, float duration)
    {
        this.nextState = nextState;
        this.duration = duration;
    }

    public override void Enter()
    {
        remainingDuration = duration;
    }

    public override bool TryCompleteProcess(out TState newState)
    {
        remainingDuration -= Time.deltaTime;
        if (remainingDuration <= 0)
        {
            newState = nextState;
            return true;
        }

        newState = default;
        return false;
    }

    public override void Exit() { }
}