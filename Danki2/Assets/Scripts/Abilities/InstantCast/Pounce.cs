using System.Collections.Generic;
using UnityEngine;

public class Pounce : InstantCast
{
    public static readonly AbilityData BaseAbilityData = new AbilityData(0, 0, 0);
    public static readonly Dictionary<OrbType, int> GeneratedOrbs = new Dictionary<OrbType, int>();
    public const OrbType AbilityOrbType = OrbType.Aggression;
    public const string Tooltip = "Deals {DAMAGE} damage.";

    // The ai casting this ability should determine cast range
    private const int Damage = 4;
    private const float MaxMovementDuration = 0.5f;
    private const float MinMovementDuration = 0.2f;
    private const float MovementSpeedMultiplier = 3f;
    private const float DamageRadius = 2f;
    private const float PauseDuration = 0.3f;

    public Pounce(Actor owner, AbilityData abilityData) : base(owner, abilityData)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        target.y = position.y;
        Vector3 direction = target - position;

        float pounceSpeed = Owner.GetStat(Stat.Speed) * MovementSpeedMultiplier;
        float movementDuration = Mathf.Clamp(direction.magnitude / pounceSpeed, MinMovementDuration, MaxMovementDuration);

        Owner.MovementManager.LockMovement(
            movementDuration,
            pounceSpeed,
            direction,
            direction
        );

        PounceObject.Create(position, Quaternion.LookRotation(target - position));

        Owner.InterruptableAction(
            movementDuration,
            InterruptionType.Hard,
            () =>
            {
                bool hasDealtDamage = false;

                CollisionTemplateManager.Instance.GetCollidingActors(
                    CollisionTemplate.Wedge90,
                    DamageRadius,
                    Owner.transform.position,
                    Quaternion.LookRotation(Owner.transform.forward)
                ).ForEach(actor =>
                {
                    if (Owner.Opposes(actor))
                    {
                        Owner.DamageTarget(actor, Damage);
                        hasDealtDamage = true;
                    }
                });

                Owner.MovementManager.Stun(PauseDuration);
                SuccessFeedbackSubject.Next(hasDealtDamage);

                if (hasDealtDamage)
                {
                    CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
                }
            }
        );
    }
}