public class DeterministicAutoTransition<T> : AutoTransition<T>
{
    public DeterministicAutoTransition(T to, float after) : base(to, after)
    {
    }

    public override float GetTransitionTime()
    {
        return after;
    }
}