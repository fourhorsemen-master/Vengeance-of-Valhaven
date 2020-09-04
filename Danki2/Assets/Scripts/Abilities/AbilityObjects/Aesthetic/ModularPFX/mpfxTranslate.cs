using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Translate")]
public class mpfxTranslate : MPFXBehaviour
{
	[SerializeField]
	private AnimationCurve[] curves = new AnimationCurve[3];

	private Vector3 Translator;

	public override bool SetUp(GameObject InGraphic, ModularPFXComponent OwningComponent)
	{
		graphic = InGraphic;
		Translator = Vector3.zero;
		timeElapsed = 0f;
		GetEndTimeFromCurveArray(curves, out endTime);

		return true;
	}

	public override bool UpdatePFX()
	{
		timeElapsed += Time.deltaTime;
		Translator.x = curves[0].Evaluate(timeElapsed) * Time.deltaTime;
		Translator.y = curves[1].Evaluate(timeElapsed) * Time.deltaTime;
		Translator.z = curves[2].Evaluate(timeElapsed) * Time.deltaTime;

		graphic.transform.Translate(Translator);

		return base.UpdatePFX();
	}

	public override bool End()
	{
		return true;
	}
}
