using UnityEngine;

[Ability(AbilityReference.Smash, new []{"PerfectSmash"})]
public class Smash : InstantCast
{
    private const float DistanceFromCaster = 1.8f;
    private const float Radius = 1f;
    private const float PerfectSmashStunDuration = 3f;
    private const float PauseDuration = 0.3f;

    public Smash(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 _)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);
        Owner.MovementManager.Pause(PauseDuration);

        Vector3 position = Owner.transform.position;

        Vector3 directionToTarget = floorTargetPosition == position
            ? Vector3.right
            : (floorTargetPosition - position).normalized;
        Vector3 center = position + (directionToTarget * DistanceFromCaster);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Cylinder, Radius, center)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(actor =>
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;

                if (HasBonus("PerfectSmash")) actor.EffectManager.AddActiveEffect(new Stun(), PerfectSmashStunDuration);
            });

        CustomCamera.Instance.AddShake(ShakeIntensity.High);
        SmashObject.Create(center);

        SuccessFeedbackSubject.Next(hasDealtDamage);
    }
}
