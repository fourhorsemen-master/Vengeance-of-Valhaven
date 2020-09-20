using UnityEngine;
using System;

public class MPFXContextScale : MPFXContext
{
	public string hereWeAre = "nobhead";
}

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Scale")]
public class mpfxScale : MPFXBehaviour
{
	[SerializeField]
	private AnimationCurve curve = new AnimationCurve();

<<<<<<< HEAD
	public override bool SetUp(MPFXContext Context, GameObject InGraphic)
=======
	public override bool SetUp(GameObject InGraphic, ModularPFXComponent OwningComponent)
>>>>>>> master
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

	public override MPFXContext ConstructContext()
	{
		return new MPFXContextScale();
	}
}
