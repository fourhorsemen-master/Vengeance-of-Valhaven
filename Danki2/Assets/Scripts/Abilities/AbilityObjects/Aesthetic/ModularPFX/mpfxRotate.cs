using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "MPFX/Behaviour/Rotate")]
public class mpfxRotate : MPFXBehaviour
{
	[SerializeField]
	private AnimationCurve[] _curves = new AnimationCurve[3];

	private Vector3 _rotator;

	public override bool SetUp(GameObject InGraphic)
	{
		_graphic = InGraphic;
		_rotator = Vector3.zero;
		_timeElapsed = 0f;
		GetEndTimeFromCurveArray(_curves, out _endTime);

		return true;
	}

	public override bool UpdatePFX()
	{
		_timeElapsed += Time.deltaTime;
		_rotator.x = _curves[0].Evaluate(_timeElapsed) * Time.deltaTime;
		_rotator.y = _curves[1].Evaluate(_timeElapsed) * Time.deltaTime;
		_rotator.z = _curves[2].Evaluate(_timeElapsed) * Time.deltaTime;

		_graphic.transform.Rotate(_rotator);

		return base.UpdatePFX();
	}

	public override bool End()
	{
		return true;
	}
}
