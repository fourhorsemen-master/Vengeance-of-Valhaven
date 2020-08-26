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
        Vector3 direction = Owner.transform.forward * -1;
        float duration = leapDistance / leapSpeed;

        Owner.MovementManager.TryLockMovement(MovementLockType.Dash, duration, leapSpeed, direction, Owner.transform.forward);

        DisengageObject.Create(Owner.transform);

        SuccessFeedbackSubject.Next(true);

        if (HasBonus("Parting Shot"))
        {
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
