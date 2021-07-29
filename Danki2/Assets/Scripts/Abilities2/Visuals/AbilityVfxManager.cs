using UnityEngine;

public class AbilityVfxManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    [SerializeField]
    private float offsetIncrement = 0;

    private void Start()
    {
        player.AbilityService.AbilityCastSubject.Subscribe(HandleVfx);
    }

    private void HandleVfx(AbilityCastInformation abilityCastInformation)
    {
        float offset = 0;
        
        abilityCastInformation.Empowerments.ForEach(empowerment =>
        {
            SlashObject.Create(
                player.AbilitySource,
                abilityCastInformation.CastRotation * Quaternion.Euler(0, offset, 0),
                EmpowermentLookup.Instance.GetColour(empowerment)
            );

            offset += offsetIncrement;
        });
    }
}
