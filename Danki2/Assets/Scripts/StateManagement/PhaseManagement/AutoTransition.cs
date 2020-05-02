using UnityEngine;

public class AutoTransition<T>
{
    private readonly float after;
    private readonly float plusOrMinus;

    public T To { get; }

    public AutoTransition(T to, float after, float plusOrMinus = 0f)
    {
        To = to;
        this.after = after;
        this.plusOrMinus = plusOrMinus;
    }

    public float GetTransitionTime()
    {
        float time = plusOrMinus <= 0f
            ? after
            : after + Random.Range(-plusOrMinus, plusOrMinus);


        return Mathf.Max(0f, time);
    }
}