using System.Collections.Generic;
using UnityEngine;

public class AbilityVfxManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    [SerializeField]
    private float offsetIncrement = 0;

    [SerializeField]
    private Color defaultAbilityVFXColour = default;

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
            CreateVFX(EmpowermentLookup.Instance.GetColour(empowerment), offset);
            offset += offsetIncrement;
        });

        if (empowerments.Count == 0)
        {
            CreateVFX(defaultAbilityVFXColour, offset);
        }
    }

    private void CreateVFX(Color empowermentColor, float offset)
    {
        AbilityType type = AbilityLookup.Instance.GetAbilityType(player.CurrentCast.AbilityId);

        switch (type)
        {
            case AbilityType.Slash:
                SlashObject.Create(
                    player.CurrentCast.CollisionTemplateOrigin,
                    player.CurrentCast.CastRotation * Quaternion.Euler(0, offset, 0),
                    empowermentColor
                );
                break;
            case AbilityType.Thrust:
                PoisonStabVisual.Create(
                    player.CurrentCast.CollisionTemplateOrigin + player.transform.forward * offset/200,
                    player.CurrentCast.CastRotation)
                    .SetColour(empowermentColor);
                break;
            case AbilityType.Smash:
                SmashObject.Create(
                    player.CurrentCast.CollisionTemplateOrigin,
                    rotation: Quaternion.Euler(0, offset, 0),
                    colour: empowermentColor
                );
                break;
        }
    }
}
