using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiFiniteStateMachine<TState> : IAiComponent where TState : Enum
{
    private readonly Dictionary<TState, IAiComponent> states = new Dictionary<TState, IAiComponent>();

    private readonly Dictionary<TState, Dictionary<TState, ISet<IAiTrigger>>> transitions =
        new EnumDictionary<TState, Dictionary<TState, ISet<IAiTrigger>>>(() =>
            new EnumDictionary<TState, ISet<IAiTrigger>>(() => new HashSet<IAiTrigger>()));

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
        transitions[from][to].UnionWith(triggers);
        return this;
    }
    
    public void Enter()
    {
        if (!TryGetComponent(initialState, out IAiComponent aiComponent)) return;

        currentState = initialState;
        aiComponent.Enter();
        ActivateTriggers();
    }

    public void Update()
    {
        TryTransition();
        if (TryGetComponent(currentState, out IAiComponent aiComponent)) aiComponent.Update();
    }

    public void Exit()
    {
        DeactivateTriggers();
        if (TryGetComponent(currentState, out IAiComponent aiComponent)) aiComponent.Exit();
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
    }

    private void Transition(TState toState)
    {
        if (!TryGetComponent(currentState, out IAiComponent fromComponent)) return;
        if (!TryGetComponent(toState, out IAiComponent toComponent)) return;
        
        DeactivateTriggers();
        fromComponent.Exit();

        currentState = toState;
        toComponent.Enter();
        ActivateTriggers();
    }

    private bool TryGetComponent(TState state, out IAiComponent aiComponent)
    {
        if (states.TryGetValue(state, out aiComponent)) return true;

        Debug.LogError($"Cannot find AI component for state: {state.ToString()}. Ensure a component is registered with this state.");
        return false;
    }

    private void ActivateTriggers()
    {
        ForEachTrigger(t => t.Activate());
    }

    private void DeactivateTriggers()
    {
        ForEachTrigger(t => t.Deactivate());
    }

    private void ForEachTrigger(Action<IAiTrigger> action)
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
