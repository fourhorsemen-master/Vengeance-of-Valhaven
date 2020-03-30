using UnityEngine;

public class CameraShake
{
    public const float DefaultInterval = 1 / 60f;

    private float intervalTimer;

    public float Strength { get; private set; }
    public float Duration { get; private set; }
    public float Interval { get; private set; }

    public CameraShake()
    {
        Strength = 0f;
        Duration = 0f;
        Interval = 1 / 60f;
        intervalTimer = 1 / 60f;
    }

    public CameraShake(float strength, float duration, float interval = DefaultInterval)
    {
        Strength = strength / 100f;
        Duration = duration;
        Interval = interval;
        intervalTimer = interval;
    }

    public Vector3 GetShakeVector()
    {
        Vector3 direction = Vector3.zero;

        if (intervalTimer < 0f)
        {
            intervalTimer += Interval;
            float rndX = Random.Range(-Strength, Strength);
            float rndZ = Random.Range(-Strength, Strength);

            direction.x = rndX;
            direction.z = rndZ;
        }

        return direction;
    }

    public void TickDuration()
    {
        if (intervalTimer > 0f) intervalTimer -= Time.deltaTime;
        if (Duration > 0f)
        {
            Duration -= Time.deltaTime;
        }
        else
        {
            Set(0f, 0f);
        }

    }

    public void Set(float strength, float duration, float interval = DefaultInterval)
    {
        Strength = strength / 100f;
        Duration = duration;
        Interval = interval;
        intervalTimer = interval;
    }
}
