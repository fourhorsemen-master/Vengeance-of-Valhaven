using System;

public interface IObservable<T>
{
    void Subscribe(Action<T> action);
    void Next(T value);
}
