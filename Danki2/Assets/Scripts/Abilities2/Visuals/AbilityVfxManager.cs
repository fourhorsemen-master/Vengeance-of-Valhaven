using UnityEngine;

public class AbilityVfxManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private void Start()
    {
        player.AbilityService.AbilityCastSubject.Subscribe(HandleVfx);
    }

    private void HandleVfx(AbilityCastInformation abilityCastInformation)
    {
        
    }
}
