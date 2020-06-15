using System;
using UnityEngine;

[Serializable]
public class MPFXBehaviour : ScriptableObject, ImpfxCallable
{
	[SerializeField]
	protected string _behaviourName;

	[SerializeField]
	protected float _duration;

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
		return false;
	}

	virtual protected void TearDown()
	{

	}
}
