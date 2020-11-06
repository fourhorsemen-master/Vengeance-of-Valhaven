using UnityEngine;

public class RandomTimeElapsed : IAiTrigger
{
    private readonly float minTime;
    private readonly float maxTime;

    private float requiredTime;

    public RandomTimeElapsed(float minTime, float maxTime)
    {
        this.minTime = minTime;
        this.maxTime = maxTime;
    }

    public void Activate()
    {
        requiredTime = Time.time + Random.Range(minTime, maxTime);
    }

    public void Deactivate() {}

    public bool Triggers()
    {
        return Time.time >= requiredTime;
    }
}
