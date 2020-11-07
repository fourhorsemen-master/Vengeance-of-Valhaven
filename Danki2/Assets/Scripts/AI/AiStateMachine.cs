using System;
using System.Collections.Generic;

public class AiStateMachine<TState> : IAiComponent where TState : Enum
{
    private readonly EnumDictionary<TState, IAiComponent> components =
        new EnumDictionary<TState, IAiComponent>(() => new NoOpComponent());
    
    private readonly EnumDictionary<TState, EnumDictionary<TState, AiTrigger>> localTriggers =
        new EnumDictionary<TState, EnumDictionary<TState, AiTrigger>>(() =>
            new EnumDictionary<TState, AiTrigger>(new NeverTrigger()));

    private readonly EnumDictionary<TState, AiTrigger> globalTriggers =
        new EnumDictionary<TState, AiTrigger>(new NeverTrigger());

    private readonly TState initialState;
    private TState currentState;

    public AiStateMachine(TState initialState)
    {
        this.initialState = initialState;
    }

    public AiStateMachine<TState> WithComponent(TState state, IAiComponent component)
    {
        components[state] = component;
        return this;
    }

    public AiStateMachine<TState> WithTransition(TState from, TState to, AiTrigger trigger)
    {
        localTriggers[from][to] = trigger;
        return this;
    }

    public AiStateMachine<TState> WithGlobalTransition(TState to, AiTrigger trigger)
    {
        globalTriggers[to] = trigger;
        return this;
    }
    
    public void Enter()
    {
        currentState = initialState;
        ActivateTriggers();
        components[currentState].Enter();
    }

    public void Exit()
    {
        DeactivateTriggers();
        components[currentState].Exit();
    }

    public void Update()
    {
        components[currentState].Update();
        TryTransition();
    }

    private void TryTransition()
    {
        foreach (KeyValuePair<TState, AiTrigger> potentialTransition in localTriggers[currentState])
        {
            TState toState = potentialTransition.Key;
            AiTrigger trigger = potentialTransition.Value;
            
            if (trigger.Triggers())
            {
                Transition(toState);
                return;
            }
        }
        
        foreach (KeyValuePair<TState, AiTrigger> potentialTransition in globalTriggers)
        {
            TState toState = potentialTransition.Key;
            AiTrigger trigger = potentialTransition.Value;

            if (toState.Equals(currentState))
            {
                continue;
            }

            if (trigger.Triggers())
            {
                Transition(toState);
                return;
            }
        }
    }

    private void Transition(TState toState)
    {
        DeactivateTriggers();
        components[currentState].Exit();

        currentState = toState;
        ActivateTriggers();
        components[currentState].Enter();
    }

    private void ActivateTriggers()
    {
        ForEachGlobalTrigger(t => t.Activate());
        ForEachLocalTrigger(t => t.Activate());
    }

    private void DeactivateTriggers()
    {
        ForEachGlobalTrigger(t => t.Deactivate());
        ForEachLocalTrigger(t => t.Deactivate());
    }

    private void ForEachGlobalTrigger(Action<AiTrigger> action)
    {
        foreach (AiTrigger trigger in globalTriggers.Values)
        {
            action(trigger);
        }
    }

    private void ForEachLocalTrigger(Action<AiTrigger> action)
    {
        foreach (AiTrigger trigger in localTriggers[currentState].Values)
        {
            action(trigger);
        }
    }
}
