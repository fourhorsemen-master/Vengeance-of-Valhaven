public interface Behaviour<T> where T : Actor
{
    void Behave(T actor);
}
