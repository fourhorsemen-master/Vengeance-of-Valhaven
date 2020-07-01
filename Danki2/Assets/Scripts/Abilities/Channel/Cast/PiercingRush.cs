﻿using UnityEngine;

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

    private PiercingRushObject piercingRushObject;

    public PiercingRush(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void End(Vector3 target)
    {        
        piercingRushObject = PiercingRushObject.Create(Owner.transform);

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
                    MultiplicativeStatModification slow = new MultiplicativeStatModification(Stat.Speed, dazeSlowMultiplier);
                    actor.EffectManager.AddActiveEffect(slow, dazeSlowTime);
                }
            }
        });


        // Jetstream.
        if (HasBonus("Jetstream"))
        {
            Owner.WaitAndAct(dashDuration + jetstreamCastDelay, () => Jetstream(piercingRushObject));
        }
        else
        {
            Owner.WaitAndAct(dashDuration, () => piercingRushObject.Destroy());
        }

        SuccessFeedbackSubject.Next(hasDealtDamage);

        Owner.WaitAndAct(dashDuration, () => Owner.MovementManager.Stun(abilityConcludedStun));
    }

    private void Jetstream(PiercingRushObject piercingRushObject)
    {
        piercingRushObject.PlayJetstreamSoundThenDestroy();

        Quaternion castRotation = Owner.transform.rotation.Backwards();

        bool hasDealtDamage = false;

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
                hasDealtDamage = true;
            }
        });

        if (hasDealtDamage) CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }
}
