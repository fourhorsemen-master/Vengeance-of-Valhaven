﻿using System;
using System.Collections.Generic;
using System.Linq;

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
        currentState = initialState;
        states[currentState].Enter();
    }

    public void Update()
    {
        if (TryTransition()) return;
        states[currentState].Update();
    }

    private bool TryTransition()
    {
        foreach (KeyValuePair<TState, ISet<IAiTrigger>> potentialTransition in transitions[currentState])
        {
            TState toState = potentialTransition.Key;
            ISet<IAiTrigger> triggers = potentialTransition.Value;
            
            if (triggers.Any(t => t.Triggers()))
            {
                currentState = toState;
                states[currentState].Enter();
                return true;
            }
        }

        return false;
    }
}
