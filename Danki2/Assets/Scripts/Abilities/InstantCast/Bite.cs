using UnityEngine;

[Ability(AbilityReference.Bite)]
public class Bite : InstantCast
{
    public const float Range = 2f;
    private const float PauseDuration = 0.3f;

    public Bite(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        Vector3 castDirection = target - Owner.Centre;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        BiteObject.Create(Owner.transform, ((Wolf) Owner).isAlpha);

        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Pause(PauseDuration);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Wedge45, Range, position, castRotation)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(actor =>
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            });

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }

        SuccessFeedbackSubject.Next(hasDealtDamage);
    }
}
