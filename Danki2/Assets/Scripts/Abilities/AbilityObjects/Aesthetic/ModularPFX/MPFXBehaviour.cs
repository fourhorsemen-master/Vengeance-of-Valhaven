using System;
using UnityEngine;

[Serializable]
public abstract class MPFXBehaviour : ScriptableObject
{
	virtual public void SetUp(MPFXContext context, GameObject inGraphic)
	{
		context.graphic = inGraphic;
		context.timeElapsed = 0f;
	}

	public bool UpdatePFX(MPFXContext context)
	{
		context.timeElapsed += Time.deltaTime;
		UpdateInternal(context);
		return context.timeElapsed > context.endTime;
	}

	protected abstract void UpdateInternal(MPFXContext context);

	protected static void GetEndTimeFromCurve(AnimationCurve InCurve, out float OutTime)
	{
		
		OutTime = InCurve.length > 0 ? InCurve.keys[InCurve.keys.Length - 1].time : 0f;
	}

	protected static void GetEndTimeFromCurveArray(AnimationCurve[] inCurves, out float outTime)
	{
		float maxSeenTime = 0f;

		foreach(AnimationCurve curve in inCurves)
		{
			GetEndTimeFromCurve(curve, out float thisCurveTime);

			maxSeenTime = Mathf.Max(maxSeenTime, thisCurveTime);
		}

		outTime = maxSeenTime;
	}

	virtual public MPFXContext ConstructContext()
	{
		return new MPFXContext();
	}
}
