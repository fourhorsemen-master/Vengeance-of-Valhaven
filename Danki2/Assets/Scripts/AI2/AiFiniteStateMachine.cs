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

    public AiFiniteStateMachine<TState> WithTransition(TState from, TState to, IAiTrigger trigger)
    {
        transitions[from][to].Add(trigger);
        return this;
    }
    
    public void Enter()
    {
        Transition(initialState);
    }

    public void Update()
    {
        if (TryTransition()) return;
        if (TryGetComponent(currentState, out IAiComponent aiComponent)) aiComponent.Update();
    }

    private bool TryTransition()
    {
        foreach (KeyValuePair<TState, ISet<IAiTrigger>> potentialTransition in transitions[currentState])
        {
            TState toState = potentialTransition.Key;
            ISet<IAiTrigger> triggers = potentialTransition.Value;
            
            if (triggers.Any(t => t.Triggers()))
            {
                Transition(toState);
                return true;
            }
        }

        return false;
    }

    private void Transition(TState toState)
    {
        if (!TryGetComponent(toState, out IAiComponent aiComponent)) return;

        currentState = toState;
        aiComponent.Enter();
        InitialiseTriggers();
    }

    private bool TryGetComponent(TState state, out IAiComponent aiComponent)
    {
        if (states.TryGetValue(state, out aiComponent)) return true;

        Debug.LogError($"Cannot find AI component for state: {state.ToString()}. Ensure a component is registered with this state.");
        return false;
    }

    private void InitialiseTriggers()
    {
        foreach (KeyValuePair<TState, ISet<IAiTrigger>> transition in transitions[currentState])
        {
            ISet<IAiTrigger> triggers = transition.Value;

            foreach (IAiTrigger trigger in triggers)
            {
                trigger.Initialise();
            }
        }
    }
}
