using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Absolute Position")]
public class mpfxAbsolutePosition : MPFXBehaviour
{
    [SerializeField]
    private AnimationCurve[] curves = new AnimationCurve[3];

    public override void SetUp(MPFXContext context, GameObject inGraphic)
    {
        base.SetUp(context, inGraphic);
        GetEndTimeFromCurveArray(curves, out context.endTime);
    }

    protected override void UpdateInternal(MPFXContext context)
    {
        Vector3 translator;
        translator.x = curves[0].Evaluate(context.timeElapsed);
        translator.y = curves[1].Evaluate(context.timeElapsed);
        translator.z = curves[2].Evaluate(context.timeElapsed);

        context.graphic.transform.position = context.owningComponent.transform.position + translator;
    }
}
