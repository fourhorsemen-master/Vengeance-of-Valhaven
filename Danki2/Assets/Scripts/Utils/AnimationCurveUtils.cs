using System.Linq;
using UnityEngine;

public static class AnimationCurveUtils
{
    public static float GetEndTime(params AnimationCurve[] animationCurves)
    {
        return animationCurves.Max(c => c.length > 0 ? c.keys[c.length - 1].time : 0);
    }
}
