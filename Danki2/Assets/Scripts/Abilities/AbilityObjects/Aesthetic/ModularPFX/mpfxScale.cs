using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Scale")]
public class mpfxScale : MPFXBehaviour
{
	[SerializeField]
	private AnimationCurve curve = new AnimationCurve();

	public override bool SetUp(MPFXContext Context, GameObject InGraphic)
	{
		Context.graphic = InGraphic;
		Context.timeElapsed = 0f;
		GetEndTimeFromCurve(curve, out Context.endTime);
		Context.graphic.transform.localScale = Vector3.one * curve.Evaluate(0f);

		return true;
	}

	public override bool UpdatePFX(MPFXContext Context)
	{
		Context.timeElapsed += Time.deltaTime;
		Context.graphic.transform.localScale = Vector3.one * curve.Evaluate(Context.timeElapsed);

		return base.UpdatePFX(Context);
	}

	public override bool End(MPFXContext Context)
	{
		return true;
	}
}
