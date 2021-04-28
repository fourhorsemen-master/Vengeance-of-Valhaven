using UnityEngine;

public static class MathUtils
{
    public static float GetDeltaTimeLerpAmount(float smoothFactor)
    {
        return 1 - Mathf.Exp(-Time.deltaTime / smoothFactor);
    }
}
