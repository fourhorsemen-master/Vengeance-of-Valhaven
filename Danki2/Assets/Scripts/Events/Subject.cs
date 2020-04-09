using System;
using System.Collections.Generic;

/// <summary>
/// A stream of events that will not emit an initial value and does not require one for construction.
/// </summary>
public class Subject<T> : IObservable<T>
{
    private readonly List<Action<T>> actions = new List<Action<T>>();

    public void Subscribe(Action<T> action)
    {
        actions.Add(action);
    }

    public void Next(T value)
    {
        actions.ForEach(a => a(value));
    }
}

public class Subject : IObservable
{
    private readonly List<Action> actions = new List<Action>();

    public void Subscribe(Action action)
    {
        actions.Add(action);
    }

    public void Next()
    {
        actions.ForEach(a => a());
    }
}
