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
        Vector3 castDirection = target - position;
        castDirection.y = 0f;

        int enemiesHit = 0;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            Range,
            position,
            Quaternion.LookRotation(castDirection)
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                DealPrimaryDamage(actor);
                enemiesHit++;
            }
        });

        Heal(enemiesHit);

        bool hasDealtDamage = enemiesHit > 0;

        SuccessFeedbackSubject.Next(hasDealtDamage);

        LeechingStrikeObject leechingStrikeObject = LeechingStrikeObject.Create(position, Quaternion.LookRotation(castDirection));

        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Stun(PauseDuration);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            leechingStrikeObject.PlayHitSound();
        }
    }
}
