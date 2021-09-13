using UnityEngine;

public class AbilityCameraShakeManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;
    
    private void Start()
    {
        player.AbilityService.AbilityEventSubject
            .Where(x => x.CastEvent == CastEvent.Impact)
            .Subscribe(HandleCameraShake);
    }

    private void HandleCameraShake(AbilityCastInformation abilityCastInformation)
    {
        ShakeIntensity shakeIntensity = abilityCastInformation.HasDealtDamage
            ? ShakeIntensity.Medium
            : ShakeIntensity.Low;
        CustomCamera.Instance.AddShake(shakeIntensity);
    }
}
