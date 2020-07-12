using System;
using System.Collections.Generic;

/// <summary>
/// A stream of events that will not emit an initial value and does not require one for construction.
/// </summary>
public class Subject : IObservable
{
    private readonly List<Subscription> subscriptions = new List<Subscription>();

    /// <inheritdoc/>
    public Subscription Subscribe(Action action)
    {
        Subscription subscription = new Subscription(action);
        subscriptions.Add(subscription);

        return subscription;
    }

    /// <inheritdoc/>
    public void Next()
    {
        subscriptions.RemoveAll(s =>
        {
            if (!s.Unsubscribed) s.Action();

            return s.Unsubscribed;
        });
    }

    /// <inheritdoc/>
    public IObservable<T> Map<T>(Func<T> mappingFunction)
    {
        Subject<T> mappedSubject = new Subject<T>();
        Subscribe(() => mappedSubject.Next(mappingFunction()));
        return mappedSubject;
    }
}

/// <inheritdoc cref="Subject"/>
/// <typeparam name="T"> The type of object that will be emitted by this subject. </typeparam>
public class Subject<T> : IObservable<T>
{
    private readonly List<Subscription<T>> subscriptions = new List<Subscription<T>>();

    /// <inheritdoc/>
    public Subscription<T> Subscribe(Action<T> action)
    {
        Subscription<T> subscription = new Subscription<T>(action);
        subscriptions.Add(subscription);

        return subscription;
    }

    /// <inheritdoc/>
    public void Next(T value)
    {
        subscriptions.RemoveAll(s =>
        {
            if (!s.Unsubscribed) s.Action(value);

            return s.Unsubscribed;
        });
    }

    /// <inheritdoc/>
    public IObservable<TMapped> Map<TMapped>(Func<T, TMapped> mappingFunction)
    {
        Subject<TMapped> mappedSubject = new Subject<TMapped>();
        Subscribe(value => mappedSubject.Next(mappingFunction(value)));
        return mappedSubject;
    }
}
