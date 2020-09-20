﻿using System;
using UnityEngine;

[Serializable]
public class MPFXBehaviour : ScriptableObject, ImpfxCallable
{
<<<<<<< HEAD
	virtual public bool SetUp(MPFXContext Context, GameObject InGraphic)
=======
	[SerializeField]
	protected string behaviourName;

	protected float timeElapsed;
	protected float endTime;

	protected GameObject graphic;

	virtual public bool SetUp(GameObject InGraphic, ModularPFXComponent OwningComponent)
>>>>>>> master
	{
		return false;
	}

	virtual public bool UpdatePFX(MPFXContext Context)
	{
		return Context.timeElapsed > Context.endTime; 
	}

	virtual public bool End(MPFXContext Context)
	{
		return false;
	}

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
		return null;
	}
}
