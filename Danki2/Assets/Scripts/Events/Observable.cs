using System;
using System.Collections.Generic;

public class Observable<T>
{
    private readonly List<Action<T>> actions = new List<Action<T>>();

    public void Subscribe(Action<T> action)
    {
        actions.Add(action);
    }

    public void Next(T value)
    {
        actions.ForEach(a => a.Invoke(value));
    }
}
