using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Scale")]
public class mpfxScale : MPFXBehaviour
{
	[SerializeField]
	private AnimationCurve _curve = new AnimationCurve();

	private float _endTime;
	private float _timeElapsed;

	public override bool SetUp(GameObject InGraphic)
	{
		Debug.Log("Setting up scale behaviour");
		_graphic = InGraphic;
		_timeElapsed = 0f;
		_endTime = _curve.keys[_curve.keys.Length - 1].time;
		_graphic.transform.localScale = Vector3.one * _curve.Evaluate(0f);

		return true;
	}

	public override bool UpdatePFX()
	{
		Debug.Log("Updating scale behaviour");
		_timeElapsed += Time.deltaTime;
		Debug.Log("Time elapsed: " + _timeElapsed.ToString());
		_graphic.transform.localScale = Vector3.one * _curve.Evaluate(_timeElapsed);
		Debug.Log("Scale decided: " + _graphic.transform.localScale.x.ToString());

		if (_timeElapsed > _endTime)
		{
			return true;
		} 

		return false;
	}

	public override bool End()
	{
		Debug.Log("Ending scale behaviour");
		TearDown();
		return true;
	}

	protected override void TearDown()
	{
		Debug.Log("Tearing down scale behaviour");
		//Is this needed?
	}
}
