using UnityEngine;

public class AbilityCameraShakeManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;
    
    private void Start()
    {
        player.AbilityAnimationListener.ImpactSubject
            .Subscribe(HandleCameraShake);
    }

    private void HandleCameraShake()
    {
        ShakeIntensity shakeIntensity = player.AbilityService.HasDealtDamage
            ? ShakeIntensity.Medium
            : ShakeIntensity.Low;
        CustomCamera.Instance.AddShake(shakeIntensity);
    }
}
