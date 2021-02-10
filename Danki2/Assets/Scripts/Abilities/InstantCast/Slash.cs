using UnityEngine;

[Ability(AbilityReference.Slash)]
public class Slash : InstantCast
{
    private const float Range = 4f;
    private const float PauseDuration = 0.3f;

    public Slash(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Vector3 castDirection = floorTargetPosition - Owner.transform.position;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Wedge90, Range, Owner.CollisionTemplateSource, castRotation)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(actor =>
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            });

        SuccessFeedbackSubject.Next(hasDealtDamage);

        SlashObject slashObject = SlashObject.Create(Owner.AbilitySource, castRotation);

        Owner.MovementManager.LookAt(floorTargetPosition);
        Owner.MovementManager.Pause(PauseDuration);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            slashObject.PlayHitSound();
        }
    }
}
