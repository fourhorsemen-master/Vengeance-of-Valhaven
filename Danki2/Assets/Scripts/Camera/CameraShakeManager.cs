using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShakeManager
{
    private CameraShake[] camShakes;

    public CameraShakeManager(int poolSize)
    {
        camShakes = new CameraShake[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            camShakes[i] = new CameraShake();
        }
    }

    public Vector3 GetShakeVector()
    {
        float maxStrength = float.MinValue;
        CameraShake strongestShake = default;

        foreach (CameraShake cs in camShakes)
        {
            if (cs.Duration > 0f && cs.Strength > maxStrength)
            {
                maxStrength = cs.Strength;
                strongestShake = cs;
            }
        }

        if (strongestShake != default)
        {
            return strongestShake.GetShakeVector();
        }

        return Vector3.zero;
    }

    public void AddCameraShake(float strength, float duration, float frequency = CameraShake.DefaultInterval)
    {
        if (!TryGetExpiredCameraShake(out var cameraShake))
        {
            return;
        }

        cameraShake.Set(strength, duration, frequency);
    }

    private bool TryGetExpiredCameraShake(out CameraShake expiredShake)
    {
        foreach (CameraShake cs in camShakes)
        {
            if (cs.Duration <= 0f)
            {
                expiredShake = cs;
                return true;
            }
        }

        expiredShake = default;
        return false;
    }
}
