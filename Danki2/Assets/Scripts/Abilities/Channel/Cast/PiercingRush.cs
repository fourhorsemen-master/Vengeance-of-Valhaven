using UnityEngine;

[Ability(AbilityReference.PiercingRush, new[] { "Daze", "Jetstream" })]
public class PiercingRush : Cast
{
    protected override float CastTime => 2f;

    private const float minimumCastRange = 2f;
    private const float maximumCastRange = 10f;
    private const float dashSpeedMultiplier = 6f;

    private const float dazeSlowModifier = 2f;
    private const float dazeSlowTime = 3f;

    private const float jetstreamCastDelay = 0.2f;

    private const float abilityConcludedStun = 0.2f;

    public PiercingRush(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void End(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        Vector3 direction = target - position;
        direction.y = position.y;

        float distance = Vector3.Distance(target, position);
        distance = Mathf.Clamp(distance, minimumCastRange, maximumCastRange);

        float dashSpeed = Owner.GetStat(Stat.Speed) * dashSpeedMultiplier;
        float duration = distance / dashSpeed;
        
        Owner.MovementManager.TryLockMovement(MovementLockType.Dash, duration, dashSpeed, direction, direction);


        SuccessFeedbackSubject.Next(true);
    }
}
