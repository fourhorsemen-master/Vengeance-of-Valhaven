using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionAnimator : MonoBehaviour
{
	[SerializeField]
    private LocomotionAnimData animData = null;

	[SerializeField]
    private Animator animator = null;

	private Vector3 positionLastFrame;

    // Start is called before the first frame update
    void Start()
    {
		if (animator == null)
		{
			Debug.LogError("LocomotionAnimator on a GameObject with no MovementManager");
		}
	}

	// Update is called once per frame
	void Update()
	{
		AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
		float currentForwardValue = 0f;

		if (currentState.IsName("Locomotion_Blend"))
		{
			Vector3 delta = gameObject.transform.position - positionLastFrame;
			float speed = delta.magnitude;

			currentForwardValue = speed / (animData.ForwardRunSpeed * Time.deltaTime);
		}

		animator.SetFloat("Forward_MoveSpeed_Blend", currentForwardValue, 0.1f, Time.deltaTime);
		//animator.SetFloat("Lateral_MoveSpeed_Blend", currentLateralValue, 0.1f, Time.deltaTime);

		if (gameObject.name.Contains("Player"))
		{
			Debug.Log("Forward: " + currentForwardValue);
		}

		positionLastFrame = gameObject.transform.position;
	}
}
