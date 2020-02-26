using System;

/// <summary>
/// Represents a stream of events that can be emitted, with Next(), and subscribed to, with Subscribe().
/// </summary>
/// <typeparam name="T"> The type of object that will be emitted. </typeparam>
public interface IObservable<T>
{
    void Subscribe(Action<T> action);
    void Next(T value);
}

public interface IObservable
{
    void Subscribe(Action action);
    void Next();
}
