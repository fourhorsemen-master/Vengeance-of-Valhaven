using UnityEngine;

public class LazyFollow : MonoBehaviour
{
	[SerializeField]
	private Transform parent;

	[SerializeField, Range(0f, 1f), Tooltip("0 will never move, 1 will follow the parent exactly.")]
	private float followStrength;

	private Vector3
		initialPositionRelativeToParent,
		initialForwardRelativeToParent,
		initialUpRelativeToParent;

	private Vector3 velocity = Vector3.zero;

	void Start()
	{
		initialPositionRelativeToParent = parent.transform.InverseTransformPoint(transform.position);
		initialForwardRelativeToParent = parent.transform.InverseTransformDirection(transform.forward);
		initialUpRelativeToParent = parent.transform.InverseTransformDirection(transform.up);

		transform.SetParent(null, true);
	}

	void Update()
	{
		Move();
		Rotate();
	}

    private void Move()
	{
		Vector3 desiredPosition = parent.transform.TransformPoint(initialPositionRelativeToParent);

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
		Vector3 desiredForward = parent.transform.TransformDirection(initialForwardRelativeToParent);
		Vector3 desiredUp = parent.transform.TransformDirection(initialUpRelativeToParent);
		Quaternion desiredRotation = Quaternion.LookRotation(desiredForward, desiredUp);

		transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * 5);
	}
}
