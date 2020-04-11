using System;

/// <summary>
/// Represents a subscription to an observable. The main functionality of this class is to
/// allow unsubscribing from the observable in question.
/// </summary>
public class Subscription<T>
{
    public Action<T> Action { get; }
    public bool Unsubscribed { get; private set; }

    public Subscription(Action<T> action)
    {
        Action = action;
        Unsubscribed = false;
    }

    public void Unsubscribe()
    {
        Unsubscribed = true;
    }
}

public class Subscription
{
    public Action Action { get; }
    public bool Unsubscribed { get; private set; }

    public Subscription(Action action)
    {
        Action = action;
        Unsubscribed = false;
    }

    public void Unsubscribe()
    {
        Unsubscribed = true;
    }
}
