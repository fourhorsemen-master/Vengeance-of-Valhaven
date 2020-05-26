using UnityEngine;

[Ability(AbilityReference.Leap)]
public class Leap : InstantCast
{
    private const float MinMovementDuration = 0.1f;
    private const float MaxMovementDuration = 0.3f;
    private const float LungeSpeedMultiplier = 6f;

    public Leap(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        Vector3 direction = target - position;
        direction.y = position.y;

        float distance = Vector3.Distance(target, position);
        float lungeSpeed = Owner.GetStat(Stat.Speed) * LungeSpeedMultiplier;
        float duration = Mathf.Clamp(distance / lungeSpeed, MinMovementDuration, MaxMovementDuration);

        Owner.MovementManager.LockMovement(duration, lungeSpeed, direction, direction);

        LeapObject.Create(Owner.transform);

        SuccessFeedbackSubject.Next(true);
    }
}
