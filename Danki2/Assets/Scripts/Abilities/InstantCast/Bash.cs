using UnityEngine;

[Ability(AbilityReference.Bash)]
public class Bash : InstantCast
{
    private const float StunDuration = 1f;
    private const float Range = 1.8f;
    private const float Radius = 0.8f;
    private const float PauseDuration = 0.3f;

    public Bash(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses)
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

        TemplateCollision(
            CollisionTemplateShape.Cylinder,
            Radius,
            center,
            Quaternion.identity,
            actor =>
            {
                DealPrimaryDamage(actor);
                actor.EffectManager.AddActiveEffect(ActiveEffect.Stun, StunDuration);
                hasDealtDamage = true;
            }
        );

        SuccessFeedbackSubject.Next(hasDealtDamage);
        
        BashObject.Create(center, hasDealtDamage);
        
        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }
}
