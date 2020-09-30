using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Fade")]
public class mpfxFade : MPFXBehaviour
{
	private class MPFXContextFade : MPFXContext
	{
		public Color originalColour;
		public Material mpfxMat;
	}

	[SerializeField]
	private AnimationCurve curve = new AnimationCurve();

	public override void SetUp(MPFXContext context, GameObject inGraphic)
	{
		base.SetUp(context, inGraphic);
		MPFXContextFade castedContext = (MPFXContextFade)context;
		castedContext.mpfxMat = castedContext.graphic.GetComponent<MeshRenderer>().material;
		castedContext.originalColour = castedContext.mpfxMat.GetColor(ModularPFXComponent.ColourKeyString);
		GetEndTimeFromCurve(curve, out castedContext.endTime);
		UpdateOpacity(castedContext);
	}

	protected override void UpdateInternal(MPFXContext context)
	{
		MPFXContextFade castedContext = (MPFXContextFade)context;
		UpdateOpacity(castedContext);
	}

	private void UpdateOpacity(MPFXContextFade castedContext)
	{
		float fadeFactor = curve.Evaluate(castedContext.timeElapsed);

		Color desiredColour = castedContext.originalColour;
		desiredColour.a *= fadeFactor;

		castedContext.mpfxMat.SetColor(ModularPFXComponent.ColourKeyString, desiredColour);
	}

	public override MPFXContext ConstructContext()
	{
		return new MPFXContextFade();
	}
}
