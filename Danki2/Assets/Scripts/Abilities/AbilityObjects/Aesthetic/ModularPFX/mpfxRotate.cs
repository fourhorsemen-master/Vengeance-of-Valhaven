using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Rotate")]
public class mpfxRotate : MPFXBehaviour
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
		Vector3 rotator;
		rotator.x = curves[0].Evaluate(context.timeElapsed) * Time.deltaTime;
		rotator.y = curves[1].Evaluate(context.timeElapsed) * Time.deltaTime;
		rotator.z = curves[2].Evaluate(context.timeElapsed) * Time.deltaTime;

		context.graphic.transform.Rotate(rotator);
	}
}
