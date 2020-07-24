using System;
using System.Collections.Generic;

/// <summary>
/// A stream of events that will emit an initial value and requires one for construction.
/// </summary>
/// <typeparam name="T"> The type of object that will be emitted by this subject. </typeparam>
public class BehaviourSubject<T> : IObservable<T>
{
    private T currentValue;

    private readonly List<Subscription<T>> subscriptions = new List<Subscription<T>>();

    public BehaviourSubject(T initialValue)
    {
        currentValue = initialValue;
    }

    /// <inheritdoc/>
    public Subscription<T> Subscribe(Action<T> action)
    {
        action(currentValue);

        Subscription<T> subscription = new Subscription<T>(action);
        subscriptions.Add(subscription);

        return subscription;
    }

    /// <inheritdoc/>
    public void Next(T value)
    {
        currentValue = value;

        subscriptions.RemoveAll(s =>
        {
            if (!s.Unsubscribed) s.Action(currentValue);

            return s.Unsubscribed;
        });
    }

    /// <inheritdoc/>
    public IObservable<TMapped> Map<TMapped>(Func<T, TMapped> mappingFunction)
    {
        BehaviourSubject<TMapped> mappedBehaviourSubject = new BehaviourSubject<TMapped>(mappingFunction(currentValue));
        Subscribe(value => mappedBehaviourSubject.Next(mappingFunction(value)));
        return mappedBehaviourSubject;
    }
}
