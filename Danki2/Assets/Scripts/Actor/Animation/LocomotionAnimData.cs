using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocomotionData", menuName = "ScriptableObjects/LocomotionAnimData", order = 1)]
public class LocomotionAnimData : ScriptableObject
{
	[SerializeField]
	private float forwardRunSpeed = 2f;
	public float ForwardRunSpeed => forwardRunSpeed;

	[SerializeField]
	private float backwardRunSpeed = -1f;
	public float BackwardRunSpeed => backwardRunSpeed;

	[SerializeField]
	private float strafeRunSpeed = 0.5f;
	public float StrafeRunSpeed => strafeRunSpeed;

}
