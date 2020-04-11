using System;
using System.Collections.Generic;

/// <summary>
/// A stream of events that will not emit an initial value and does not require one for construction.
/// </summary>
public class Subject<T> : IObservable<T>
{
    private readonly List<Subscription<T>> subscriptions = new List<Subscription<T>>();

    public Subscription<T> Subscribe(Action<T> action)
    {
        Subscription<T> subscription = new Subscription<T>(action);
        subscriptions.Add(subscription);

        return subscription;
    }

    public void Next(T value)
    {
        subscriptions.RemoveAll(s =>
        {
            if (!s.Unsubscribed) s.Action(value);

            return s.Unsubscribed;
        });
    }
}

public class Subject : IObservable
{
    private readonly List<Subscription> subscriptions = new List<Subscription>();

    public Subscription Subscribe(Action action)
    {
        Subscription subscription = new Subscription(action);
        subscriptions.Add(subscription);

        return subscription;
    }

    public void Next()
    {
        subscriptions.RemoveAll(s =>
        {
            if (!s.Unsubscribed) s.Action();

            return s.Unsubscribed;
        });
    }
}
