using System;
using UnityEngine;

[Serializable]
public abstract class MPFXBehaviour : ScriptableObject
{
	virtual public void SetUp(MPFXContext Context, GameObject InGraphic)
	{
		Context.graphic = InGraphic;
		Context.timeElapsed = 0f;
	}

	public bool UpdatePFX(MPFXContext Context)
	{
		Context.timeElapsed += Time.deltaTime;
		UpdateInternal(Context);
		return Context.timeElapsed > Context.endTime;
	}

	protected abstract void UpdateInternal(MPFXContext Context);

	protected static void GetEndTimeFromCurve(AnimationCurve InCurve, out float OutTime)
	{
		
		OutTime = InCurve.length > 0 ? InCurve.keys[InCurve.keys.Length - 1].time : 0f;
	}

	protected static void GetEndTimeFromCurveArray(AnimationCurve[] InCurves, out float OutTime)
	{
		float maxSeenTime = 0f;

		foreach(AnimationCurve curve in InCurves)
		{
			GetEndTimeFromCurve(curve, out float thisCurveTime);

			maxSeenTime = Mathf.Max(maxSeenTime, thisCurveTime);
		}

		OutTime = maxSeenTime;
	}

	virtual public MPFXContext ConstructContext()
	{
		return new MPFXContext();
	}
}
