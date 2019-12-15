public interface IBehaviour<T> where T : Actor
{
    void Behave(T actor);
}
