using UnityEngine;

public static class MathUtils
{
    public static float GetDeltaTimeLerpAmount(float smoothFactor)
    {
        float power = -Time.deltaTime / smoothFactor;
        return float.IsNaN(power) ? 0 : 1 - Mathf.Exp(power);
    }
}
