﻿using UnityEngine;

[Ability(AbilityReference.PiercingRush, new[] { "Daze", "Jetstream" })]
public class PiercingRush : Cast
{
    private const float minimumCastRange = 2f;
    private const float maximumCastRange = 10f;
    private const float dashDamageWidth = 4.6f;
    private const float dashDamageHeight = 5f;
    private const float dashSpeedMultiplier = 6f;

    private const float DazeSlowDuration = 2f;

    private const float jetstreamCastDelay = 0.2f;
    private const float jetstreamRange = 3f;

    private const float postDashPauseDuration = 0.2f;

    private readonly Subject onCastCancelled = new Subject();
    private readonly Subject<float> onCastComplete = new Subject<float>();

    public override ChannelEffectOnMovement EffectOnMovement => ChannelEffectOnMovement.Root;

    public PiercingRush(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses, float duration)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses, duration)
    {
    }

    protected override void Start()
    {
        PiercingRushObject.Create(Owner.transform, onCastCancelled, onCastComplete);
    }

    protected override void Cancel() => onCastCancelled.Next();

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        // Dash.
        Vector3 position = Owner.transform.position;
        Vector3 direction = floorTargetPosition - position;

        float distance = Vector3.Distance(floorTargetPosition, position);
        distance = Mathf.Clamp(distance, minimumCastRange, maximumCastRange);

        float dashSpeed = Owner.StatsManager.Get(Stat.Speed) * dashSpeedMultiplier;
        float dashDuration = distance / dashSpeed;

        Owner.MovementManager.TryLockMovement(MovementLockType.Dash, dashDuration, dashSpeed, direction, direction);
        Owner.StartTrail(dashDuration);
        onCastComplete.Next(dashDuration);


        // Dash damage and Daze.
        Vector3 collisionDetectionScale = new Vector3(dashDamageWidth, dashDamageHeight, distance);

        Vector3 collisionDetectionPosition = position;
        collisionDetectionPosition += Owner.transform.forward.normalized * distance / 2;

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
                DealDamageDuringRush(actor, direction, dashSpeed);                
                hasDealtDamage = true;
            }
        });

        // Jetstream.
        if (HasBonus("Jetstream")) Owner.WaitAndAct(dashDuration + jetstreamCastDelay, () => Jetstream());

        SuccessFeedbackSubject.Next(hasDealtDamage);

        Owner.WaitAndAct(dashDuration, () => Owner.MovementManager.Pause(postDashPauseDuration));
    }

    private void DealDamageDuringRush(Actor enemy, Vector3 rushDirection, float rushSpeed)
    {
        Vector3 ownerToEnemy = enemy.transform.position - Owner.transform.position;

        float passingDistance = Vector3.Dot(ownerToEnemy, rushDirection.normalized);
        float passingTime = passingDistance / rushSpeed;

        Owner.InterruptibleAction(passingTime, InterruptionType.Hard, () =>
        {
            DealPrimaryDamage(enemy);
            CustomCamera.Instance.AddShake(ShakeIntensity.High);

            if (HasBonus("Daze"))
            {
                enemy.EffectManager.AddActiveEffect(ActiveEffect.Slow, DazeSlowDuration);
            }
        });
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
