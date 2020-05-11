using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.Lunge)]
public class Lunge : InstantCast
{
    private const float MinMovementDuration = 0.1f;
    private const float MaxMovementDuration = 0.2f;
    private const float LungeSpeedMultiplier = 6f;
    private const float StunRange = 2f;
    private const float StunDuration = 0.5f;
    private const float PauseDuration = 0.3f;

    public Lunge(Actor owner, AbilityData abilityData) : base(owner, abilityData)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        Vector3 direction = target - position;
        direction.y = position.y;

        float distance = Vector3.Distance(target, position);
        float lungeSpeed = Owner.GetStat(Stat.Speed) * LungeSpeedMultiplier;
        float duration = Mathf.Clamp(distance/lungeSpeed, MinMovementDuration, MaxMovementDuration);

        Owner.MovementManager.LockMovement(duration, lungeSpeed, direction, direction );

        LungeObject lungeObject = LungeObject.Create(position, Quaternion.LookRotation(target - position));

        Owner.InterruptableAction(
            duration,
            InterruptionType.Hard,
            () =>
            {
                bool hasDealtDamage = false;

                CollisionTemplateManager.Instance.GetCollidingActors(
                    CollisionTemplate.Wedge90,
                    StunRange,
                    Owner.transform.position,
                    Quaternion.LookRotation(direction)
                ).ForEach(actor =>
                {
                    if (Owner.Opposes(actor))
                    {
                        actor.EffectManager.AddActiveEffect(new Stun(StunDuration), StunDuration);
                        DealPrimaryDamage(actor);
                        hasDealtDamage = true;
                    }
                });

                SuccessFeedbackSubject.Next(hasDealtDamage);
                Owner.MovementManager.Stun(PauseDuration);

                if (hasDealtDamage)
                {
                    CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
                    lungeObject.PlayHitSound();
                }
            }
        );
    }
}
