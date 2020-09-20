using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Absolute Position")]
public class mpfxAbsolutePosition : MPFXBehaviour
{
    [SerializeField]
    private AnimationCurve[] curves = new AnimationCurve[3];

    public override bool SetUp(MPFXContext Context, GameObject InGraphic)
    {
        Context.graphic = InGraphic;
        Context.timeElapsed = 0f;
        GetEndTimeFromCurveArray(curves, out Context.endTime);

        return true;
    }

    public override bool UpdatePFX(MPFXContext Context)
    {
        Context.timeElapsed += Time.deltaTime;
        Vector3 translator;
        translator.x = curves[0].Evaluate(Context.timeElapsed);
        translator.y = curves[1].Evaluate(Context.timeElapsed);
        translator.z = curves[2].Evaluate(Context.timeElapsed);

        Context.graphic.transform.position = Context.owningComponent.transform.position + translator;

        return base.UpdatePFX(Context);
    }

    public override bool End(MPFXContext Context)
    {
        return true;
    }
}
