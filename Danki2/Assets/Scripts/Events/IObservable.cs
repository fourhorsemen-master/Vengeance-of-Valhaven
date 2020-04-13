using System;

/// <summary>
/// Represents a stream of events that can be subscribed to with Subscribe(). An event can be emitted with Next().
/// </summary>
public interface IObservable
{
    /// <summary>
    /// Subscribe to event emissions from this observable.
    /// </summary>
    Subscription Subscribe(Action action);
    
    /// <summary>
    /// Emit a new event from this observable.
    /// </summary>
    void Next();
}

/// <inheritdoc cref="IObservable"/>
/// <typeparam name="T"> The type of object that will be emitted. </typeparam>
public interface IObservable<T>
{
    /// <inheritdoc cref="IObservable.Subscribe"/>
    Subscription<T> Subscribe(Action<T> action);
    
    /// <inheritdoc cref="IObservable.Next"/>
    void Next(T value);
}
