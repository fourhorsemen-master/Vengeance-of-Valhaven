using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/WorldSpaceOrientation")]
public class mpfxWorldSpaceOrientation : MPFXBehaviour
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

		float xOrientation = curves[0].Evaluate(Context.timeElapsed);
		float yOrientation = curves[1].Evaluate(Context.timeElapsed);
		float zOrientation = curves[2].Evaluate(Context.timeElapsed);

		Context.graphic.transform.rotation = Quaternion.Euler(xOrientation, yOrientation, zOrientation);

		return base.UpdatePFX(Context);
	}

	public override bool End(MPFXContext Context)
	{
		return true;
	}
}