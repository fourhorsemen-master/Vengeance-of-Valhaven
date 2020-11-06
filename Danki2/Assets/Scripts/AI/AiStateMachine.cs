using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiStateMachine<TState> : IAiComponent where TState : Enum
{
    private readonly EnumDictionary<TState, IAiComponent> components =
        new EnumDictionary<TState, IAiComponent>(() => new NoOpComponent());

    private readonly EnumDictionary<TState, EnumDictionary<TState, ISet<AiTrigger>>> localTriggers =
        new EnumDictionary<TState, EnumDictionary<TState, ISet<AiTrigger>>>(() =>
            new EnumDictionary<TState, ISet<AiTrigger>>(() => new HashSet<AiTrigger>()));

    private readonly EnumDictionary<TState, ISet<AiTrigger>> globalTriggers =
        new EnumDictionary<TState, ISet<AiTrigger>>(() => new HashSet<AiTrigger>());

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

    public AiStateMachine<TState> WithTransition(TState from, TState to, params AiTrigger[] triggers)
    {
        localTriggers[from][to].UnionWith(triggers);
        return this;
    }

    public AiStateMachine<TState> WithGlobalTransition(TState to, params AiTrigger[] triggers)
    {
        globalTriggers[to].UnionWith(triggers);
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
        foreach (KeyValuePair<TState, ISet<AiTrigger>> potentialTransition in localTriggers[currentState])
        {
            TState toState = potentialTransition.Key;
            ISet<AiTrigger> triggers = potentialTransition.Value;
            
            if (triggers.Any(t => t.Triggers()))
            {
                Transition(toState);
                return;
            }
        }
        
        foreach (KeyValuePair<TState,ISet<AiTrigger>> potentialTransition in globalTriggers)
        {
            TState toState = potentialTransition.Key;
            ISet<AiTrigger> triggers = potentialTransition.Value;

            if (toState.Equals(currentState))
            {
                continue;
            }

            if (triggers.Any(t => t.Triggers()))
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
        foreach (ISet<AiTrigger> triggers in globalTriggers.Values)
        {
            foreach (AiTrigger trigger in triggers)
            {
                action(trigger);
            }
        }
    }

    private void ForEachLocalTrigger(Action<AiTrigger> action)
    {
        foreach (ISet<AiTrigger> triggers in localTriggers[currentState].Values)
        {
            foreach (AiTrigger trigger in triggers)
            {
                action(trigger);
            }
        }
    }
}
