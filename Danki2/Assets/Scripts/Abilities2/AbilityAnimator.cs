using UnityEngine;

public class AbilityAnimator : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private void Start()
    {
        player.AbilityService.AbilityCastSubject.Subscribe(HandleAnimation);
    }

    private void HandleAnimation()
    {
        string animationState = AnimationStringLookup.LookupTable[AbilityAnimationType.Slash];
        player.AnimController.Play(animationState);
    }
}
