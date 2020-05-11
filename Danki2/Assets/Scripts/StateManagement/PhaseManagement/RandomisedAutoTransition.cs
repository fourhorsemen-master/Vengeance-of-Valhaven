using UnityEngine;

public class RandomisedAutoTransition<T> : AutoTransition<T>
{
    private readonly float plusOrMinus;

    public RandomisedAutoTransition(T to, float after, float plusOrMinus) : base(to, after)
    {
        this.plusOrMinus = plusOrMinus;
    }

    public override float GetTransitionTime()
    {
        return Mathf.Max(0f, after + Random.Range(-plusOrMinus, plusOrMinus));
    }
}