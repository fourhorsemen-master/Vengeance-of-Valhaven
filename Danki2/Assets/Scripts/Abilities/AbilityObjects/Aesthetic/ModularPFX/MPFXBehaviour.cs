using System;
using UnityEngine;

[Serializable]
public class MPFXBehaviour : ScriptableObject, ImpfxCallable
{
	[SerializeField]
	protected string _behaviourName;

	[SerializeField]
	protected float _endTime;

	[SerializeField]
	protected float _timeElapsed;

	protected GameObject _graphic;

	virtual public bool End()
	{
		return false;
	}

	virtual public bool SetUp(GameObject InGraphic)
	{
		return false;
	}

	virtual public bool UpdatePFX()
	{
		if(_timeElapsed > _endTime)
		{
			return true;
		}

		return false;
	}

	virtual protected void TearDown()
	{

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
			float thisCurveTime = 0f;
			GetEndTimeFromCurve(curve, out thisCurveTime);

			maxSeenTime = Mathf.Max(maxSeenTime, thisCurveTime);
		}

		OutTime = maxSeenTime;
	}
}
