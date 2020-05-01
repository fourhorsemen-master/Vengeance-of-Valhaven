using UnityEngine;
using System;
using System.Collections.Generic;

public class PhaseManager<T> where T : Enum
{
    private readonly MonoBehaviour routineHost;
    private readonly StateManager<T> stateManager;
    private readonly Dictionary<T, Tuple<float, T>> autoTransitionDictionary = new Dictionary<T, Tuple<float, T>>();
    private Coroutine autoTransition = null;

    public T CurrentState => stateManager.CurrentState;

    public PhaseManager(MonoBehaviour routineHost, T initialPhase, Action transitionAction = null)
    {
        this.routineHost = routineHost;
        this.stateManager = new StateManager<T>(initialPhase, transitionAction);
    }

    public void Transition(T to)
    {
        if (autoTransition != null)
        {
            routineHost.StopCoroutine(autoTransition);
        }

        stateManager.Transition(to);

        if (autoTransitionDictionary.TryGetValue(to, out Tuple<float, T> value))
        {
            autoTransition = routineHost.WaitAndAct(value.Item1, () => Transition(value.Item2));
        }
    }

    public PhaseManager<T> WithTransition(T from, T to, float transitionAutomaticallyAfter = -1f)
    {
        stateManager.WithTransition(from, to);

        if (transitionAutomaticallyAfter > 0f)
        {
            autoTransitionDictionary.Add(from, new Tuple<float, T>(transitionAutomaticallyAfter, to));
        }

        return this;
    }
}