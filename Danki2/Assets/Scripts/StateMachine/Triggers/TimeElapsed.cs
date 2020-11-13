using UnityEngine;

public class TimeElapsed : StateMachineTrigger
{
    private readonly float time;

    private float requiredTime;

    public TimeElapsed(float time)
    {
        this.time = time;
    }

    public override void Activate()
    {
        requiredTime = Time.time + time;
    }

    public override void Deactivate() {}

    public override bool Triggers()
    {
        return Time.time >= requiredTime;
    }
}
