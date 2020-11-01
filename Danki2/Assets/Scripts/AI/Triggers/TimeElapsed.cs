using UnityEngine;

public class TimeElapsed : IAiTrigger
{
    private readonly float time;

    private float requiredTime;

    public TimeElapsed(float time)
    {
        this.time = time;
    }

    public void Activate()
    {
        requiredTime = Time.time + time;
    }

    public void Deactivate() {}

    public bool Triggers()
    {
        return Time.time >= requiredTime;
    }
}
