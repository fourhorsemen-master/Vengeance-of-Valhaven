using System;
using UnityEngine;

public class TimeElapsedProcessor<TState> : Processor<TState> where TState : Enum
{
    private readonly TState nextState;
    private readonly float duration;
    private float requiredTime;

    public TimeElapsedProcessor(TState nextState, float duration)
    {
        this.nextState = nextState;
        this.duration = duration;
    }

    public void Enter()
    {
        requiredTime = Time.time + duration;
    }

    public void Exit() { }

    public bool TryCompleteProcess(out TState newState)
    {
        if (Time.time >= requiredTime)
        {
            newState = nextState;
            return true;
        }

        newState = default;
        return false;
    }
}