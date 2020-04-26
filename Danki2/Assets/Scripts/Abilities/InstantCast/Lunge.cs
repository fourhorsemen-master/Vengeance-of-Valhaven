using UnityEngine;

public class Lunge : InstantCast
{
    private const float LungeDuration = 0.2f;
    private const float LungeSpeedMultiplier = 6f;
    private const int LungeDamage = 4;
    private const float StunRange = 2f;
    private const float StunDuration = 0.5f;
    private const float PauseDuration = 0.3f;

    public Lunge(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Actor owner = Context.Owner;
        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;

        Vector3 direction = target - position;
        direction.y = position.y;

        owner.MovementManager.LockMovement(
            LungeDuration,
            owner.GetStat(Stat.Speed) * LungeSpeedMultiplier,
            direction,
            direction
        );

        LungeObject lungeObject = LungeObject.Create(position, Quaternion.LookRotation(target - position));

        owner.WaitAndAct(LungeDuration, () =>
        {
            bool hasDealtDamage = false;

            CollisionTemplateManager.Instance.GetCollidingActors(
                CollisionTemplate.Wedge90,
                StunRange,
                owner.transform.position,
                Quaternion.LookRotation(direction)
            ).ForEach(actor =>
            {
                if (owner.Opposes(actor))
                {
                    actor.EffectManager.AddActiveEffect(new Stun(StunDuration), StunDuration);
                    actor.ModifyHealth(-LungeDamage);
                    hasDealtDamage = true;
                }
            });

            SuccessFeedbackSubject.Next(hasDealtDamage);
            owner.MovementManager.Stun(PauseDuration);

            if (hasDealtDamage)
            {
                CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
                lungeObject.PlayHitSound();
            }
        });
    }
}
