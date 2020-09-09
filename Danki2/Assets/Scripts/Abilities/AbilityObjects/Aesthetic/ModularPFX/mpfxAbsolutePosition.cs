using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Absolute Position")]
public class mpfxAbsolutePosition : MPFXBehaviour
{
    [SerializeField]
    private AnimationCurve[] curves = new AnimationCurve[3];

    private Vector3 translator;

    private ModularPFXComponent owningComponent;

    public override bool SetUp(GameObject InGraphic, ModularPFXComponent OwningComponent)
    {
        graphic = InGraphic;
        translator = Vector3.zero;
        timeElapsed = 0f;
        owningComponent = OwningComponent;
        GetEndTimeFromCurveArray(curves, out endTime);

        return true;
    }

    public override bool UpdatePFX()
    {
        timeElapsed += Time.deltaTime;
        translator.x = curves[0].Evaluate(timeElapsed);
        translator.y = curves[1].Evaluate(timeElapsed);
        translator.z = curves[2].Evaluate(timeElapsed);

        graphic.transform.position = owningComponent.transform.position + translator;

        return base.UpdatePFX();
    }

    public override bool End()
    {
        return true;
    }
}
