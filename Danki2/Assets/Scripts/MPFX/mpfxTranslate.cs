using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Translate")]
public class mpfxTranslate : MPFXBehaviour
{
	[SerializeField]
	private AnimationCurve[] curves = new AnimationCurve[3];

	public override void SetUp(MPFXContext context, GameObject inGraphic)
	{
		base.SetUp(context, inGraphic);
		context.endTime = AnimationCurveUtils.GetEndTime(curves);
	}

	protected override void UpdateInternal(MPFXContext context)
	{
		Vector3 Translator;
		Translator.x = curves[0].Evaluate(context.timeElapsed) * Time.deltaTime;
		Translator.y = curves[1].Evaluate(context.timeElapsed) * Time.deltaTime;
		Translator.z = curves[2].Evaluate(context.timeElapsed) * Time.deltaTime;

		context.graphic.transform.Translate(Translator);
	}
}
