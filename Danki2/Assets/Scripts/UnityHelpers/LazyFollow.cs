using UnityEngine;

public class LazyFollow : MonoBehaviour
{
	[SerializeField]
	private Transform parent;

	[SerializeField, Range(0f, 1f)]
	private float followStrength;

	private Vector3 initialLocalPosition;

	private Quaternion initialLocalRotation;

	private Vector3 velocity = Vector3.zero;

	void Start()
	{
		initialLocalPosition = transform.localPosition;
		initialLocalRotation = transform.localRotation;

		transform.SetParent(null, true);
	}

	void Update()
	{
		Move();
		Rotate();
	}

    private void Move()
	{
		Vector3 desiredPosition = parent.transform.TransformPoint(initialLocalPosition);

		// Acceleration proportional to square of distance from desired position
		velocity += Vector3.Distance(desiredPosition, transform.position)
			* (desiredPosition - transform.position)
			* followStrength
			* Time.deltaTime;

		// Dampen velocity to stop endless oscillation
		velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 2);

		transform.position += velocity;
	}

	private void Rotate()
	{
		Quaternion desiredRotation = parent.rotation * initialLocalRotation;

		transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * 5);
	}
}
