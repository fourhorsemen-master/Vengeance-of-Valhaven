using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiFiniteStateMachine<TState> : IAiComponent where TState : Enum
{
    private readonly EnumDictionary<TState, IAiComponent> states =
        new EnumDictionary<TState, IAiComponent>(() => new NoOpAiComponent());

    private readonly EnumDictionary<TState, Dictionary<TState, ISet<IAiTrigger>>> transitions =
        new EnumDictionary<TState, Dictionary<TState, ISet<IAiTrigger>>>(() =>
            new EnumDictionary<TState, ISet<IAiTrigger>>(() => new HashSet<IAiTrigger>()));

    private readonly EnumDictionary<TState, ISet<IAiTrigger>> globalTransitions =
        new EnumDictionary<TState, ISet<IAiTrigger>>(() => new HashSet<IAiTrigger>());

    private readonly TState initialState;
    private TState currentState;

    public AiFiniteStateMachine(TState initialState)
    {
        this.initialState = initialState;
    }

    public AiFiniteStateMachine<TState> WithState(TState state, IAiComponent component)
    {
        states[state] = component;
        return this;
    }

    public AiFiniteStateMachine<TState> WithTransition(TState from, TState to, params IAiTrigger[] triggers)
    {
        if (from.Equals(to))
        {
            Debug.LogError($"Cannot add transition with same from and to state: {from.ToString()}.");
            return this;
        }
        
        transitions[from][to].UnionWith(triggers);
        return this;
    }

    public AiFiniteStateMachine<TState> WithGlobalTransition(TState to, params IAiTrigger[] triggers)
    {
        globalTransitions[to].UnionWith(triggers);
        return this;
    }
    
    public void Enter()
    {
        currentState = initialState;
        states[currentState].Enter();
        ActivateCurrentTriggers();
    }

    public void Update()
    {
        TryTransition();
        states[currentState].Update();
    }

    public void Exit()
    {
        DeactivateCurrentTriggers();
        states[currentState].Exit();
    }

    private void TryTransition()
    {
        foreach (KeyValuePair<TState, ISet<IAiTrigger>> potentialTransition in transitions[currentState])
        {
            TState toState = potentialTransition.Key;
            ISet<IAiTrigger> triggers = potentialTransition.Value;
            
            if (triggers.Any(t => t.Triggers()))
            {
                Transition(toState);
                return;
            }
        }
        
        foreach (KeyValuePair<TState,ISet<IAiTrigger>> globalTransition in globalTransitions)
        {
            TState toState = globalTransition.Key;
            ISet<IAiTrigger> triggers = globalTransition.Value;

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
        DeactivateCurrentTriggers();
        states[currentState].Exit();

        currentState = toState;
        states[currentState].Enter();
        ActivateCurrentTriggers();
    }

    private void ActivateCurrentTriggers()
    {
        ForEachCurrentTrigger(t => t.Activate());
    }

    private void DeactivateCurrentTriggers()
    {
        ForEachCurrentTrigger(t => t.Deactivate());
    }

    private void ForEachCurrentTrigger(Action<IAiTrigger> action)
    {
        foreach (ISet<IAiTrigger> triggers in transitions[currentState].Values)
        {
            foreach (IAiTrigger trigger in triggers)
            {
                action(trigger);
            }
        }
    }
}
