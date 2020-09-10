using UnityEngine;

[Ability(AbilityReference.Disengage, new[] { "Parting Shot" })]
public class Disengage : InstantCast
{
    private const float leapSpeed = 14f;
    private const float leapDistance = 6f;

    private const float partingShotRange = 3.2f;

    public Disengage(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 travelDirection = Owner.transform.position - target;
        Vector3 faceDirection = target - Owner.transform.position;
        float duration = leapDistance / leapSpeed;

        Owner.MovementManager.TryLockMovement(MovementLockType.Dash, duration, leapSpeed, travelDirection, faceDirection);
        Owner.StartTrail(duration);

        DisengageObject.Create(Owner.transform, duration);

        SuccessFeedbackSubject.Next(true);

        if (HasBonus("Parting Shot"))
        {
            SmashObject.Create(Owner.transform.position, false);

            CollisionTemplateManager.Instance.GetCollidingActors(
                CollisionTemplate.Cylinder,
                partingShotRange,
                Owner.transform.position
            ).ForEach(actor =>
            {
                if (Owner.Opposes(actor))
                {
                    DealPrimaryDamage(actor);
                }
            });
        }
    }
}
