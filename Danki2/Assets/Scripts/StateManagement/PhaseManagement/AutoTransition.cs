public abstract class AutoTransition<T>
{
    protected readonly float after;

    public T To { get; }

    protected AutoTransition(T to, float after)
    {
        To = to;
        this.after = after;
    }

    public abstract float GetTransitionTime();
}