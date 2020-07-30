using UnityEngine;

[Ability(AbilityReference.LeechingStrike)]
public class LeechingStrike : InstantCast
{
    private const float Range = 4f;
    private const float PauseDuration = 0.3f;

    public LeechingStrike(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        Vector3 castDirection = target - Owner.Centre;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        int enemiesHit = 0;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Wedge90, Range, position, castRotation)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(actor =>
            {
                DealPrimaryDamage(actor);
                enemiesHit++;
            });

        bool hasDealtDamage = enemiesHit > 0;

        if (hasDealtDamage) Heal(enemiesHit);

        SuccessFeedbackSubject.Next(hasDealtDamage);

        LeechingStrikeObject leechingStrikeObject = LeechingStrikeObject.Create(position, castRotation);

        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Pause(PauseDuration);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            leechingStrikeObject.PlayHitSound();
        }
    }
}
