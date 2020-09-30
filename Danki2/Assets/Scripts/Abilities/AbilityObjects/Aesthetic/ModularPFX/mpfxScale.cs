using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Scale")]
public class mpfxScale : MPFXBehaviour
{
	[SerializeField]
	private AnimationCurve curve = new AnimationCurve();

	public override void SetUp(MPFXContext context, GameObject inGraphic)
	{
		base.SetUp(context, inGraphic);
		GetEndTimeFromCurve(curve, out context.endTime);
		context.graphic.transform.localScale = Vector3.one * curve.Evaluate(0f);
	}

	protected override void UpdateInternal(MPFXContext context)
	{
		context.graphic.transform.localScale = Vector3.one * curve.Evaluate(context.timeElapsed);
	}
}
