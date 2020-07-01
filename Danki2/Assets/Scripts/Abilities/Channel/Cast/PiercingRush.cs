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

    public PiercingRush(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void End(Vector3 target)
    {
        SuccessFeedbackSubject.Next(true);


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
        Vector3 collisionDetectionOffset = Owner.transform.forward.normalized * distance / 2;
        collisionDetectionPosition += collisionDetectionOffset;

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

                if (HasBonus("Daze"))
                {
                    MultiplicativeStatModification slow = new MultiplicativeStatModification(Stat.Speed, dazeSlowMultiplier);
                    actor.EffectManager.AddActiveEffect(slow, dazeSlowTime);
                }
            }
        });


        // Jetstream.
        if (HasBonus("Jetstream"))
        {
            Owner.WaitAndAct(dashDuration + jetstreamCastDelay, () => Jetstream());
        }

        Owner.MovementManager.Stun(abilityConcludedStun);
    }

    private void Jetstream()
    {
        Vector3 faceDirection = Owner.transform.rotation.eulerAngles;
        faceDirection = new Vector3(faceDirection.x, faceDirection.y + 180f, faceDirection.z);
        Quaternion castRotation = Quaternion.Euler(faceDirection);

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            jetstreamRange,
            Owner.transform.position,
            castRotation
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                DealPrimaryDamage(actor);
            }
        });
    }
}
