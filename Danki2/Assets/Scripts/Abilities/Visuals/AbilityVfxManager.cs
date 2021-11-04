using System.Collections.Generic;
using UnityEngine;

public class AbilityVfxManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    [SerializeField]
    private float offsetIncrement = 0;

    private void Start()
    {
        player.AbilityAnimationListener.ImpactSubject
            .Subscribe(HandleVfx);
    }

    private void HandleVfx()
    {
        float offset = 0;

        List<Empowerment> empowerments = player.CurrentCast.Empowerments;

        empowerments.ForEach(empowerment =>
        {
            CreateVFX(empowerment, offset);
            offset += offsetIncrement;
        });
    }

    private void CreateVFX(Empowerment empowerment, float offset)
    {
        AbilityType type = AbilityLookup.Instance.GetAbilityType(player.CurrentCast.AbilityId);

        switch (type)
        {
            case AbilityType.Slash:
                SlashObject.Create(
                    player.CurrentCast.CollisionTemplateOrigin,
                    player.CurrentCast.CastRotation * Quaternion.Euler(0, offset, 0),
                    EmpowermentLookup.Instance.GetColour(empowerment)
                );
                break;
            case AbilityType.Thrust:
                PoisonStabVisual.Create(
                    player.CurrentCast.CollisionTemplateOrigin + player.transform.forward * offset/200,
                    player.CurrentCast.CastRotation)
                    .SetColour(EmpowermentLookup.Instance.GetColour(empowerment));
                break;
            case AbilityType.Smash:
                SmashObject.Create(
                    player.CurrentCast.CollisionTemplateOrigin,
                    rotation: Quaternion.Euler(0, offset, 0),
                    colour: EmpowermentLookup.Instance.GetColour(empowerment)
                );
                break;
        }
    }
}
