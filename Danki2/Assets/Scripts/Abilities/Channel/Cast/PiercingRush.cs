using UnityEngine;

[Ability(AbilityReference.PiercingRush, new[] { "Daze", "Jetstream" })]
public class PiercingRush : Cast
{
    protected override float CastTime => 2f;

    private const float minimumCastRange = 2f;
    private const float maximumCastRange = 10f;
    private const float dashDamageWidth = 6f;
    private const float dashDamageHeight = 5f;
    private const float dashSpeedMultiplier = 6f;

    private const float dazeSlowMultiplier = 0.5f;
    private const float dazeSlowTime = 3f;

    private const float jetstreamCastDelay = 0.2f;
    private const float jetstreamRange = 3f;

    private const float abilityConcludedStun = 0.2f;

    public override ChannelEffectOnMovement EffectOnMovement => ChannelEffectOnMovement.Root;

    public PiercingRush(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void End(Vector3 target)
    {
        // Dash.
        Vector3 position = Owner.transform.position;
        Vector3 direction = target - position;
        direction.y = position.y;

        float distance = Vector3.Distance(target, position);
        distance = Mathf.Clamp(distance, minimumCastRange, maximumCastRange);

        float dashSpeed = Owner.GetStat(Stat.Speed) * dashSpeedMultiplier;
        float dashDuration = distance / dashSpeed;
        
        Owner.MovementManager.TryLockMovement(MovementLockType.Dash, dashDuration, dashSpeed, direction, direction);


        // Dash damage and Daze.
        Vector3 collisionDetectionScale = new Vector3(dashDamageWidth, dashDamageHeight, distance);

        Vector3 collisionDetectionPosition = position;
        Vector3 collisionDetectionOffset = position + Owner.transform.forward.normalized * distance / 2;

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Cuboid,
            collisionDetectionScale,
            collisionDetectionPosition,
            Quaternion.LookRotation(direction)
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                DealPrimaryDamage(actor);
                CustomCamera.Instance.AddShake(ShakeIntensity.High);
                hasDealtDamage = true;

                if (HasBonus("Daze"))
                {
                    actor.EffectManager.AddActiveEffect(
                        new MultiplicativeStatModification(Stat.Speed, dazeSlowMultiplier),
                        dazeSlowTime
                    );
                }
            }
        });


        // Jetstream.
        if (HasBonus("Jetstream")) Owner.WaitAndAct(dashDuration + jetstreamCastDelay, () => Jetstream());

        PiercingRushObject.Create(Owner.transform, HasBonus("Jetstream"), dashDuration);

        SuccessFeedbackSubject.Next(hasDealtDamage);

        Owner.WaitAndAct(dashDuration, () => Owner.MovementManager.Pause(abilityConcludedStun));
    }

    private void Jetstream()
    {
        Quaternion castRotation = Owner.transform.rotation.Backwards();

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Wedge90, jetstreamRange, Owner.transform.position, castRotation)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(actor =>
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            });

        if (hasDealtDamage) CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }
}
