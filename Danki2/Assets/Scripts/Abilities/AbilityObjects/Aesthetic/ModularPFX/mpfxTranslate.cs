using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Translate")]
public class mpfxTranslate : MPFXBehaviour
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
		Vector3 Translator;
		Translator.x = curves[0].Evaluate(Context.timeElapsed) * Time.deltaTime;
		Translator.y = curves[1].Evaluate(Context.timeElapsed) * Time.deltaTime;
		Translator.z = curves[2].Evaluate(Context.timeElapsed) * Time.deltaTime;

		Context.graphic.transform.Translate(Translator);

		return base.UpdatePFX(Context);
	}

	public override bool End(MPFXContext Context)
	{
		return true;
	}
}
