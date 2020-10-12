using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/WorldSpaceOrientation")]
public class mpfxWorldSpaceOrientation : MPFXBehaviour
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
		float xOrientation = curves[0].Evaluate(context.timeElapsed);
		float yOrientation = curves[1].Evaluate(context.timeElapsed);
		float zOrientation = curves[2].Evaluate(context.timeElapsed);

		context.graphic.transform.rotation = Quaternion.Euler(xOrientation, yOrientation, zOrientation);
	}
}