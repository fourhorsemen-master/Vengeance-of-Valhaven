using System;
using System.Collections.Generic;

public class StateMachine<TState> : StateMachineComponent where TState : Enum
{
    private readonly EnumDictionary<TState, StateMachineComponent> components =
        new EnumDictionary<TState, StateMachineComponent>(() => new NoOpComponent());
    
    private readonly EnumDictionary<TState, EnumDictionary<TState, StateMachineTrigger>> localTriggers =
        new EnumDictionary<TState, EnumDictionary<TState, StateMachineTrigger>>(() =>
            new EnumDictionary<TState, StateMachineTrigger>(new NeverTrigger()));

    private readonly EnumDictionary<TState, StateMachineTrigger> globalTriggers =
        new EnumDictionary<TState, StateMachineTrigger>(new NeverTrigger());

    private readonly TState initialState;
    private TState currentState;

    public StateMachine(TState initialState)
    {
        this.initialState = initialState;
    }

    public StateMachine<TState> WithComponent(TState state, StateMachineComponent component)
    {
        components[state] = component;
        return this;
    }

    public StateMachine<TState> WithTransition(TState from, TState to, StateMachineTrigger stateMachineTrigger)
    {
        localTriggers[from][to] = stateMachineTrigger;
        return this;
    }

    public StateMachine<TState> WithGlobalTransition(TState to, StateMachineTrigger stateMachineTrigger)
    {
        globalTriggers[to] = stateMachineTrigger;
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
        foreach (KeyValuePair<TState, StateMachineTrigger> potentialTransition in localTriggers[currentState])
        {
            TState toState = potentialTransition.Key;
            StateMachineTrigger stateMachineTrigger = potentialTransition.Value;
            
            if (stateMachineTrigger.Triggers())
            {
                Transition(toState);
                return;
            }
        }
        
        foreach (KeyValuePair<TState, StateMachineTrigger> potentialTransition in globalTriggers)
        {
            TState toState = potentialTransition.Key;
            StateMachineTrigger stateMachineTrigger = potentialTransition.Value;

            if (toState.Equals(currentState))
            {
                continue;
            }

            if (stateMachineTrigger.Triggers())
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

    private void ForEachGlobalTrigger(Action<StateMachineTrigger> action)
    {
        foreach (StateMachineTrigger trigger in globalTriggers.Values)
        {
            action(trigger);
        }
    }

    private void ForEachLocalTrigger(Action<StateMachineTrigger> action)
    {
        foreach (StateMachineTrigger trigger in localTriggers[currentState].Values)
        {
            action(trigger);
        }
    }
}
