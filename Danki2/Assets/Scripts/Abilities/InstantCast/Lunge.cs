using UnityEngine;

public class Lunge : InstantCast
{
    public static readonly AbilityData BaseAbilityData = new AbilityData(0, 0, 0);

    private const float LungeDuration = 0.2f;
    private const float LungeSpeedMultiplier = 6f;
    private const int Damage = 4;
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

        Owner.MovementManager.LockMovement(
            LungeDuration,
            Owner.GetStat(Stat.Speed) * LungeSpeedMultiplier,
            direction,
            direction
        );

        LungeObject lungeObject = LungeObject.Create(position, Quaternion.LookRotation(target - position));

        Owner.InterruptableAction(
            LungeDuration,
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
                        Owner.DamageTarget(actor, Damage);
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
