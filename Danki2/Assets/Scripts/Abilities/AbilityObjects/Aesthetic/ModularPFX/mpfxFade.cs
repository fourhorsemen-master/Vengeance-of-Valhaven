using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Fade")]
public class mpfxFade : MPFXBehaviour
{
	[SerializeField]
	private AnimationCurve curve = new AnimationCurve();

	Color originalColour;

	private Material mpfxMat;

	public override bool SetUp(GameObject InGraphic, ModularPFXComponent OwningComponent)
	{
		graphic = InGraphic;
		mpfxMat = graphic.GetComponent<MeshRenderer>().material;
		originalColour = mpfxMat.GetColor(ModularPFXComponent.ColourKeyString);
		timeElapsed = 0f;
		GetEndTimeFromCurve(curve, out endTime);

		return true;
	}

	public override bool UpdatePFX()
	{
		timeElapsed += Time.deltaTime;

		float fadeFactor = curve.Evaluate(timeElapsed);

		Color desiredColour = originalColour;
		desiredColour.a *= fadeFactor;

		mpfxMat.SetColor(ModularPFXComponent.ColourKeyString, desiredColour);

		return base.UpdatePFX();
	}

	public override bool End()
	{
		return true;
	}
}
