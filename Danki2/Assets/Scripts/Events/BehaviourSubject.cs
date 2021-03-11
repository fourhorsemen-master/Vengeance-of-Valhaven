using System;
using System.Collections.Generic;

/// <summary>
/// A stream of events that will emit an initial value and requires one for construction.
/// </summary>
/// <typeparam name="T"> The type of object that will be emitted by this subject. </typeparam>
public class BehaviourSubject<T> : IObservable<T>
{
    private readonly List<Subscription<T>> subscriptions = new List<Subscription<T>>();

    public T Value { get; private set; }

    public BehaviourSubject(T initialValue)
    {
        Value = initialValue;
    }

    /// <inheritdoc/>
    public Subscription<T> Subscribe(Action<T> action)
    {
        action(Value);

        Subscription<T> subscription = new Subscription<T>(action);
        subscriptions.Add(subscription);

        return subscription;
    }

    /// <inheritdoc/>
    public void Next(T value)
    {
        Value = value;

        subscriptions.RemoveAll(s =>
        {
            if (!s.Unsubscribed) s.Action(Value);

            return s.Unsubscribed;
        });
    }

    /// <inheritdoc/>
    public IObservable<TMapped> Map<TMapped>(Func<T, TMapped> mappingFunction)
    {
        BehaviourSubject<TMapped> mappedBehaviourSubject = new BehaviourSubject<TMapped>(mappingFunction(Value));
        Subscribe(value => mappedBehaviourSubject.Next(mappingFunction(value)));
        return mappedBehaviourSubject;
    }

    /// <inheritdoc/>
    public IObservable<T> Where(Func<T, bool> filter)
    {
        if (filter(Value))
        {
            BehaviourSubject<T> filteredBehaviourSubject = new BehaviourSubject<T>(Value);
            Subscribe(value =>
            {
                if (filter(value)) filteredBehaviourSubject.Next(value);
            });
            return filteredBehaviourSubject;
        }

        Subject<T> filteredSubject = new Subject<T>();
        Subscribe(value =>
        {
            if (filter(value)) filteredSubject.Next(value);
        });
        return filteredSubject;
    }
}
