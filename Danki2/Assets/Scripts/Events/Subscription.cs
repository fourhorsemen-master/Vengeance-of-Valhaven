using System;

/// <summary>
/// Represents a subscription to an observable. The main functionality of this class is to
/// allow unsubscribing from the observable in question.
/// </summary>
public interface ISubscription
{
    /// <summary>
    /// Marks this subscription as unsubscribed. The implementation of this logic is up to
    /// the observable handling this subscription.
    /// </summary>
    void Unsubscribe();
}

/// <inheritdoc cref="ISubscription"/>
public class Subscription : ISubscription
{
    public Action Action { get; }
    public bool Unsubscribed { get; private set; }

    public Subscription(Action action)
    {
        Action = action;
        Unsubscribed = false;
    }

    /// <inheritdoc cref="ISubscription.Unsubscribe()"/>
    public void Unsubscribe()
    {
        Unsubscribed = true;
    }
}

/// <inheritdoc cref="ISubscription"/>
/// <typeparam name="T"> The type of object that this subscription is subscribing to. </typeparam>
public class Subscription<T> : ISubscription
{
    public Action<T> Action { get; }
    public bool Unsubscribed { get; private set; }

    public Subscription(Action<T> action)
    {
        Action = action;
        Unsubscribed = false;
    }

    /// <inheritdoc cref="ISubscription.Unsubscribe()"/>
    public void Unsubscribe()
    {
        Unsubscribed = true;
    }
}
