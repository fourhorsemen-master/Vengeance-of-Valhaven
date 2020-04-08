using System;
using System.Collections.Generic;

/// <summary>
/// A stream of events that will emit an initial value and requires one for construction.
/// </summary>
public class BehaviourSubject<T> : IObservable<T>
{
    private T currentValue;

    private readonly List<Action<T>> actions = new List<Action<T>>();

    public BehaviourSubject(T initialValue)
    {
        currentValue = initialValue;
    }

    public void Subscribe(Action<T> action)
    {
        action(currentValue);
        actions.Add(action);
    }

    public void Next(T value)
    {
        currentValue = value;
        actions.ForEach(a => a(currentValue));
    }
}
