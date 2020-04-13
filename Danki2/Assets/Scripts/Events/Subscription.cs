using System;

/// <summary>
/// Represents a subscription to an observable. The main functionality of this class is to
/// allow unsubscribing from the observable in question.
/// </summary>
public class Subscription
{
    public Action Action { get; }
    public bool Unsubscribed { get; private set; }

    public Subscription(Action action)
    {
        Action = action;
        Unsubscribed = false;
    }

    /// <summary>
    /// Marks this subscription as unsubscribed. The implementation of this logic is up to
    /// the observable handling this subscription.
    /// </summary>
    public void Unsubscribe()
    {
        Unsubscribed = true;
    }
}

/// <inheritdoc cref="Subscription"/>
/// <typeparam name="T"> The type of object that this subscription is subscribing to. </typeparam>
public class Subscription<T>
{
    public Action<T> Action { get; }
    public bool Unsubscribed { get; private set; }

    public Subscription(Action<T> action)
    {
        Action = action;
        Unsubscribed = false;
    }

    /// <inheritdoc cref="Subscription.Unsubscribe()"/>
    public void Unsubscribe()
    {
        Unsubscribed = true;
    }
}
