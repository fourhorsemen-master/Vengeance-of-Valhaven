using UnityEngine;

[Ability(AbilityReference.Disengage, new[] { "Parting Shot" })]
public class Disengage : InstantCast
{
    private const float MinMovementDuration = 0.1f;
    private const float MaxMovementDuration = 0.3f;
    private const float LeapSpeedMultiplier = 6f;

    private const float StunRange = 2f;
    private const float StunDuration = 3f;

    public Disengage(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
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


        Owner.MovementManager.TryLockMovement(MovementLockType.Dash, duration, leapSpeed, direction, direction);

        LeapObject.Create(Owner.transform);

        SuccessFeedbackSubject.Next(true);

        if (HasBonus("Momentum")) Owner.WaitAndAct(duration, StunSurroundingEnemies);
    }

    private void StunSurroundingEnemies()
    {
        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Cylinder,
            StunRange,
            Owner.transform.position
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                actor.EffectManager.AddActiveEffect(new Stun(), StunDuration);
            }
        });
    }
}
