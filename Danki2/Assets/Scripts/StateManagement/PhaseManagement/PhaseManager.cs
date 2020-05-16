using UnityEngine;
using System;
using System.Collections.Generic;

public class PhaseManager<T> where T : Enum
{
    private readonly MonoBehaviour routineHost;
    private readonly StateManager<T> stateManager;
    private readonly Dictionary<T, AutoTransition<T>> autoTransitionDictionary = new Dictionary<T, AutoTransition<T>>();
    private Coroutine currentTransition = null;

    public T CurrentPhase => stateManager.CurrentState;

    public PhaseManager(MonoBehaviour routineHost, T initialPhase, Action transitionAction = null)
    {
        this.routineHost = routineHost;
        this.stateManager = new StateManager<T>(initialPhase, transitionAction);
    }

    public void Transition(T to)
    {
        if (to.Equals(CurrentPhase)) return;

        if (currentTransition != null)
        {
            routineHost.StopCoroutine(currentTransition);
        }

        stateManager.Transition(to);

        if (autoTransitionDictionary.TryGetValue(to, out AutoTransition<T> transition))
        {
            currentTransition = routineHost.WaitAndAct(
                transition.GetTransitionTime(),
                () => Transition(transition.To)
            );
        }
    }

    public PhaseManager<T> WithTransition(T from, T to)
    {
        stateManager.WithTransition(from, to);

        return this;
    }

    public PhaseManager<T> WithTransition(T from, AutoTransition<T> transition)
    {
        stateManager.WithTransition(from, transition.To);

        autoTransitionDictionary.Add(from, transition);

        return this;
    }
}