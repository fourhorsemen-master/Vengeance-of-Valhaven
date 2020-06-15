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
		_graphic = InGraphic;
		_timeElapsed = 0f;
		GetEndTimeFromCurve(_curve, out _endTime);
		_graphic.transform.localScale = Vector3.one * _curve.Evaluate(0f);

		return true;
	}

	public override bool UpdatePFX()
	{
		_timeElapsed += Time.deltaTime;
		_graphic.transform.localScale = Vector3.one * _curve.Evaluate(_timeElapsed);

		return base.UpdatePFX();
	}

	public override bool End()
	{
		TearDown();
		return true;
	}

	protected override void TearDown()
	{
		//Is this needed?
	}
}
