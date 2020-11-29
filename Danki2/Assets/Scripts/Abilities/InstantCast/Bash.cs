using UnityEngine;
using UnityEngine.AI;

[Ability(AbilityReference.Bash)]
public class Bash : InstantCast
{
    private const float StunDuration = 1f;
    private const float Range = 2f;
    private const float Radius = 0.8f;
    private const float PauseDuration = 0.3f;

    public Bash(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);
        Owner.MovementManager.Pause(PauseDuration);

        Vector3 position = Owner.transform.position;

        Vector3 directionToTarget = floorTargetPosition == position
            ? Vector3.right
            : (floorTargetPosition - position).normalized;
        Vector3 center = position + (directionToTarget * Range);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Cylinder, Radius, center)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(actor =>
            {
                DealPrimaryDamage(actor);
                actor.EffectManager.AddActiveEffect(ActiveEffect.Stun, StunDuration);
                hasDealtDamage = true;
            });

        SuccessFeedbackSubject.Next(hasDealtDamage);
        
        BashObject.Create(center, hasDealtDamage);
        
        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }
}
