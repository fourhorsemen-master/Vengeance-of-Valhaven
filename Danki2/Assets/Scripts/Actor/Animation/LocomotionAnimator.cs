using UnityEngine;

public class LocomotionAnimator : MonoBehaviour
{
    [SerializeField]
    private LocomotionAnimData animData = null;

    [SerializeField]
    private Actor actor = null;

    private Vector3 positionLastFrame;

    void Update()
    {
        AnimatorStateInfo currentState = actor.Animator.GetCurrentAnimatorStateInfo(0);
        float currentForwardValue = 0f;

        if (currentState.IsName(CommonAnimStrings.Locomotion))
        {
            Vector3 delta = gameObject.transform.position - positionLastFrame;
            float speed = delta.magnitude;

            currentForwardValue = speed / (animData.ForwardRunSpeed * Time.deltaTime);
        }

        actor.Animator.SetFloat(CommonAnimStrings.ForwardBlend, currentForwardValue, 0.1f, Time.deltaTime);

        positionLastFrame = gameObject.transform.position;
    }
}
