using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Scale")]
public class mpfxScale : MPFXBehaviour
{
	[SerializeField]
	private AnimationCurve curve = new AnimationCurve();

	public override bool SetUp(GameObject InGraphic)
	{
		graphic = InGraphic;
		timeElapsed = 0f;
		GetEndTimeFromCurve(curve, out endTime);
		graphic.transform.localScale = Vector3.one * curve.Evaluate(0f);

		return true;
	}

	public override bool UpdatePFX()
	{
		timeElapsed += Time.deltaTime;
		graphic.transform.localScale = Vector3.one * curve.Evaluate(timeElapsed);

		return base.UpdatePFX();
	}

	public override bool End()
	{
		return true;
	}
}
