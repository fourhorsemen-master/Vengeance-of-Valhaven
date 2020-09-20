using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Rotate")]
public class mpfxRotate : MPFXBehaviour
{
	[SerializeField]
	private AnimationCurve[] curves = new AnimationCurve[3];

<<<<<<< HEAD
	public override bool SetUp(MPFXContext Context, GameObject InGraphic)
=======
	private Vector3 rotator;

	public override bool SetUp(GameObject InGraphic, ModularPFXComponent OwningComponent)
>>>>>>> master
	{
		Context.graphic = InGraphic;
		Context.timeElapsed = 0f;
		GetEndTimeFromCurveArray(curves, out Context.endTime);

		return true;
	}

	public override bool UpdatePFX(MPFXContext Context)
	{
		Context.timeElapsed += Time.deltaTime;
		Vector3 rotator;
		rotator.x = curves[0].Evaluate(Context.timeElapsed) * Time.deltaTime;
		rotator.y = curves[1].Evaluate(Context.timeElapsed) * Time.deltaTime;
		rotator.z = curves[2].Evaluate(Context.timeElapsed) * Time.deltaTime;

		Context.graphic.transform.Rotate(rotator);

		return base.UpdatePFX(Context);
	}

	public override bool End(MPFXContext Context)
	{
		return true;
	}
}
