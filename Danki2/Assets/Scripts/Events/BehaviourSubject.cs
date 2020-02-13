using System;
using System.Collections.Generic;

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
        action.Invoke(currentValue);
        actions.Add(action);
    }

    public void Next(T value)
    {
        currentValue = value;
        actions.ForEach(a => a.Invoke(currentValue));
    }
}
