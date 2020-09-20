using UnityEngine;
using System;

public class MPFXContextFade : MPFXContext
{
	public Color originalColour;
	public Material mpfxMat;
}

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Fade")]
public class mpfxFade : MPFXBehaviour
{
	[SerializeField]
	private AnimationCurve curve = new AnimationCurve();

	public override bool SetUp(MPFXContext Context, GameObject InGraphic)
	{
		MPFXContextFade castedContext = (MPFXContextFade)Context;
		castedContext.graphic = InGraphic;
		castedContext.mpfxMat = castedContext.graphic.GetComponent<MeshRenderer>().material;
		castedContext.originalColour = castedContext.mpfxMat.GetColor(ModularPFXComponent.ColourKeyString);
		castedContext.timeElapsed = 0f;
		GetEndTimeFromCurve(curve, out castedContext.endTime);
		UpdateOpacity(castedContext);

		return true;
	}

	public override bool UpdatePFX(MPFXContext Context)
	{
		MPFXContextFade castedContext = (MPFXContextFade)Context;
		castedContext.timeElapsed += Time.deltaTime;
		UpdateOpacity(castedContext);

		return base.UpdatePFX(Context);
	}

	public override bool End(MPFXContext Context)
	{
		return true;
	}

	private void UpdateOpacity(MPFXContextFade CastedContext)
	{
		float fadeFactor = curve.Evaluate(CastedContext.timeElapsed);

		Color desiredColour = CastedContext.originalColour;
		desiredColour.a *= fadeFactor;

		CastedContext.mpfxMat.SetColor(ModularPFXComponent.ColourKeyString, desiredColour);
	}

	public override MPFXContext ConstructContext()
	{
		return new MPFXContextFade();
	}
}
