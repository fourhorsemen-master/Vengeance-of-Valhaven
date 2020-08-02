using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Rotate")]
public class mpfxRotate : MPFXBehaviour
{
	[SerializeField]
	private AnimationCurve[] curves = new AnimationCurve[3];

	private Vector3 rotator;

	public override bool SetUp(GameObject InGraphic)
	{
		graphic = InGraphic;
		rotator = Vector3.zero;
		timeElapsed = 0f;
		GetEndTimeFromCurveArray(curves, out endTime);

		return true;
	}

	public override bool UpdatePFX()
	{
		timeElapsed += Time.deltaTime;
		rotator.x = curves[0].Evaluate(timeElapsed) * Time.deltaTime;
		rotator.y = curves[1].Evaluate(timeElapsed) * Time.deltaTime;
		rotator.z = curves[2].Evaluate(timeElapsed) * Time.deltaTime;

		graphic.transform.Rotate(rotator);

		return base.UpdatePFX();
	}

	public override bool End()
	{
		return true;
	}
}
