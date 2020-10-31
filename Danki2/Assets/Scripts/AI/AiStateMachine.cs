using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiStateMachine<TState> : IAiComponent where TState : Enum
{
    private readonly EnumDictionary<TState, IAiComponent> components =
        new EnumDictionary<TState, IAiComponent>(() => new NoOpComponent());

    private readonly EnumDictionary<TState, EnumDictionary<TState, ISet<IAiTrigger>>> localTriggers =
        new EnumDictionary<TState, EnumDictionary<TState, ISet<IAiTrigger>>>(() =>
            new EnumDictionary<TState, ISet<IAiTrigger>>(() => new HashSet<IAiTrigger>()));

    private readonly EnumDictionary<TState, ISet<IAiTrigger>> globalTriggers =
        new EnumDictionary<TState, ISet<IAiTrigger>>(() => new HashSet<IAiTrigger>());

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

    public AiStateMachine<TState> WithTransition(TState from, TState to, params IAiTrigger[] triggers)
    {
        localTriggers[from][to].UnionWith(triggers);
        return this;
    }

    public AiStateMachine<TState> WithGlobalTransition(TState to, params IAiTrigger[] triggers)
    {
        globalTriggers[to].UnionWith(triggers);
        return this;
    }
    
    public void Enter()
    {
        currentState = initialState;
        ActivateGlobalTriggers();
        ActivateLocalTriggers();
        components[currentState].Enter();
    }

    public void Exit()
    {
        DeactivateGlobalTriggers();
        DeactivateLocalTriggers();
        components[currentState].Exit();
    }

    public void Update()
    {
        components[currentState].Update();
        TryTransition();
    }

    private void TryTransition()
    {
        foreach (KeyValuePair<TState, ISet<IAiTrigger>> potentialTransition in localTriggers[currentState])
        {
            TState toState = potentialTransition.Key;
            ISet<IAiTrigger> triggers = potentialTransition.Value;
            
            if (triggers.Any(t => t.Triggers()))
            {
                Transition(toState);
                return;
            }
        }
        
        foreach (KeyValuePair<TState,ISet<IAiTrigger>> potentialTransition in globalTriggers)
        {
            TState toState = potentialTransition.Key;
            ISet<IAiTrigger> triggers = potentialTransition.Value;

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
        DeactivateGlobalTriggers();
        DeactivateLocalTriggers();
        components[currentState].Exit();

        currentState = toState;
        ActivateGlobalTriggers();
        ActivateLocalTriggers();
        components[currentState].Enter();
    }

    private void ActivateLocalTriggers()
    {
        ForEachLocalTrigger(t => t.Activate());
    }

    private void DeactivateLocalTriggers()
    {
        ForEachLocalTrigger(t => t.Deactivate());
    }

    private void ForEachLocalTrigger(Action<IAiTrigger> action)
    {
        foreach (ISet<IAiTrigger> triggers in localTriggers[currentState].Values)
        {
            foreach (IAiTrigger trigger in triggers)
            {
                action(trigger);
            }
        }
    }

    private void ActivateGlobalTriggers()
    {
        ForEachGlobalTrigger(t => t.Activate());
    }

    private void DeactivateGlobalTriggers()
    {
        ForEachGlobalTrigger(t => t.Deactivate());
    }

    private void ForEachGlobalTrigger(Action<IAiTrigger> action)
    {
        foreach (ISet<IAiTrigger> triggers in globalTriggers.Values)
        {
            foreach (IAiTrigger trigger in triggers)
            {
                action(trigger);
            }
        }
    }
}
