using System.Collections.Generic;

public abstract class Behaviour<T> where T : Actor
{
    protected Behaviour(List<float> args) { }

    public abstract void Behave(T actor);
}
