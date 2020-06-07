using UnityEngine;

[Ability(AbilityReference.Leap, new []{"Momentum"})]
public class Leap : InstantCast
{
    private const float MinMovementDuration = 0.1f;
    private const float MaxMovementDuration = 0.3f;
    private const float LeapSpeedMultiplier = 6f;

    public Leap(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        Vector3 direction = target - position;
        direction.y = position.y;

        float distance = Vector3.Distance(target, position);
        float leapSpeed = Owner.GetStat(Stat.Speed) * LeapSpeedMultiplier;
        float duration = Mathf.Clamp(distance / leapSpeed, MinMovementDuration, MaxMovementDuration);

        Owner.MovementManager.LockMovement(duration, leapSpeed, direction, direction);

        LeapObject.Create(Owner.transform);

        SuccessFeedbackSubject.Next(true);
    }
}
