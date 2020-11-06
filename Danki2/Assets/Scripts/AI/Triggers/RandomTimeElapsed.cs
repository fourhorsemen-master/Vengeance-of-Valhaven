using UnityEngine;

public class RandomTimeElapsed : AiTrigger
{
    private readonly float minTime;
    private readonly float maxTime;

    private float requiredTime;

    public RandomTimeElapsed(float minTime, float maxTime)
    {
        this.minTime = minTime;
        this.maxTime = maxTime;
    }

    public override void Activate()
    {
        requiredTime = Time.time + Random.Range(minTime, maxTime);
    }

    public override void Deactivate() {}

    public override bool Triggers()
    {
        return Time.time >= requiredTime;
    }
}
