﻿using System;
using System.Collections.Generic;
using UnityEngine;


public class StateManager<T> where T : Enum
{
    private readonly EnumDictionary<T, ISet<T>> transitions = new EnumDictionary<T, ISet<T>>(() => new HashSet<T>());
    private Action transitionAction;

    public T CurrentState { get; private set; }

    public StateManager(T initialState, Action transitionAction = null)
    {
        CurrentState = initialState;
        this.transitionAction = transitionAction ?? (() => { });
    }

    public StateManager<T> WithTransition(T from, T to)
    {
        transitions[from].Add(to);
        return this;
    }

    public bool CanTransition(T to)
    {
        return transitions[CurrentState].Contains(to);
    }

    public void Transition(T to)
    {
        if (!CanTransition(to))
        {
            Debug.LogError($"Attempted to make invalid transition from {CurrentState} to {to}");
            return;
        }

        CurrentState = to;
        transitionAction();
    }
}