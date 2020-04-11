using System;
using System.Collections.Generic;

/// <summary>
/// A stream of events that will emit an initial value and requires one for construction.
/// </summary>
public class BehaviourSubject<T> : IObservable<T>
{
    private T currentValue;

    private readonly List<Subscription<T>> subscriptions = new List<Subscription<T>>();

    public BehaviourSubject(T initialValue)
    {
        currentValue = initialValue;
    }

    public Subscription<T> Subscribe(Action<T> action)
    {
        action(currentValue);

        Subscription<T> subscription = new Subscription<T>(action);
        subscriptions.Add(subscription);

        return subscription;
    }

    public void Next(T value)
    {
        currentValue = value;

        subscriptions.RemoveAll(s =>
        {
            if (!s.Unsubscribed) s.Action(currentValue);

            return s.Unsubscribed;
        });
    }
}
