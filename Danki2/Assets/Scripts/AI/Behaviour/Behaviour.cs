public abstract class Behaviour<T> where T : Actor
{
    protected Behaviour(float[] args) { }

    public abstract void Behave(T actor);
}
