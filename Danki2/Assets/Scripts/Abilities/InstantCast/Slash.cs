using System.Linq;
using UnityEngine;

[Ability(AbilityReference.Slash)]
public class Slash : InstantCast
{
    private const float Range = 4f;
    private const float PauseDuration = 0.3f;

    public Slash(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        Vector3 castDirection = target - Owner.Centre;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Wedge90, Range, position, castRotation)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(actor =>
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            });

        SuccessFeedbackSubject.Next(hasDealtDamage);

        SlashObject slashObject = SlashObject.Create(position, castRotation);

        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Stun(PauseDuration);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            slashObject.PlayHitSound();
        }
    }
}
