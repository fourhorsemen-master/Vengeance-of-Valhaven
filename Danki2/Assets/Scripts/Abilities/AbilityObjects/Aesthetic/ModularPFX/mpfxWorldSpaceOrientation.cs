using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/WorldSpaceOrientation")]
public class mpfxWorldSpaceOrientation : MPFXBehaviour
{
	[SerializeField]
	private AnimationCurve[] curves = new AnimationCurve[3];

	public override bool SetUp(GameObject InGraphic, ModularPFXComponent OwningComponent)
	{
		graphic = InGraphic;
		timeElapsed = 0f;
		GetEndTimeFromCurveArray(curves, out endTime);

		return true;
	}

	public override bool UpdatePFX()
	{
		timeElapsed += Time.deltaTime;

		float xOrientation = curves[0].Evaluate(timeElapsed);
		float yOrientation = curves[1].Evaluate(timeElapsed);
		float zOrientation = curves[2].Evaluate(timeElapsed);

		graphic.transform.rotation = Quaternion.Euler(xOrientation, yOrientation, zOrientation);

		return base.UpdatePFX();
	}

	public override bool End()
	{
		return true;
	}
}