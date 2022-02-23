using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyFollow : MonoBehaviour
{
	[SerializeField]
	private Transform Parent;

	[SerializeField, Range(0f, 1f), Tooltip("0 will never move, 1 will follow the parent exactly.")]
	private float FollowStrength;

	private Vector3 pos, forward, up;

	private Vector3 velocity = Vector3.zero;
	void Start()
	{
		pos = Parent.transform.InverseTransformPoint(transform.position);
		forward = Parent.transform.InverseTransformDirection(transform.forward);
		up = Parent.transform.InverseTransformDirection(transform.up);

		transform.SetParent(null, true);
	}

	void Update()
	{
		Vector3 newpos = Parent.transform.TransformPoint(pos);
		Vector3 newforward = Parent.transform.TransformDirection(forward);
		Vector3 newup = Parent.transform.TransformDirection(up);
		Quaternion newrot = Quaternion.LookRotation(newforward, newup);

		velocity += Vector3.Distance(newpos, transform.position) * (newpos - transform.position) * FollowStrength * Time.deltaTime;

		velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 2);

		transform.position += velocity;

		transform.rotation = Quaternion.Lerp(transform.rotation, newrot, Time.deltaTime * 5);
	}
}
