using UnityEngine;

public class LocomotionAnimator : MonoBehaviour
{
    [SerializeField]
    private LocomotionAnimData animData = null;

    [SerializeField]
    private Actor actor = null;

    private Vector3 positionLastFrame;
    private Quaternion rotationLastFrame;

    void Update()
    {
        AnimatorStateInfo currentState = actor.Animator.GetCurrentAnimatorStateInfo(0);
        float currentForwardValue = 0f;
        float currentLateralValue = 0f;

        if (currentState.IsName(CommonAnimStrings.Locomotion))
        {
            Vector3 deltaForward = gameObject.transform.position - positionLastFrame;
            float forwardSpeedPercentage = deltaForward.magnitude;

            currentForwardValue = forwardSpeedPercentage / (animData.ForwardRunSpeed * Time.deltaTime);

            float deltaDegreesTurned =  gameObject.transform.rotation.eulerAngles.y - rotationLastFrame.eulerAngles.y;
            currentLateralValue = deltaDegreesTurned / (animData.TurnSpeed);
        }

        actor.Animator.SetFloat(CommonAnimStrings.ForwardBlend, Mathf.Clamp(currentForwardValue, -1f, 1f), 0.2f, Time.deltaTime);
        actor.Animator.SetFloat(CommonAnimStrings.TurnBlend, Mathf.Clamp(currentLateralValue, -1f, 1f), 0.2f, Time.deltaTime);

        positionLastFrame = gameObject.transform.position;
        rotationLastFrame = gameObject.transform.rotation;

        if(gameObject.name.Contains("Bear"))
        { 
             Debug.Log("Animator speed: " + actor.Animator.speed.ToString());        
        }
    }
}
