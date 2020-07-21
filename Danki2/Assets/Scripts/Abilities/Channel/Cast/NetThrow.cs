using UnityEngine;

[Ability(AbilityReference.NetThrow)]
public class NetThrow : Cast
{
    protected override float CastTime => 2f;

    private const float minimumThrowDistance = 3f;
    private const float maximumThrowDistance = 10f;

    private float gravity => Physics.gravity.magnitude;
    private float throwVelocity => Mathf.Sqrt(maximumThrowDistance * gravity); // R,max = V^2/g

    private const float netRootTime = 4f;

    public NetThrow(Actor owner, AbilityData abilityData, string[] availableBonuses)
        : base(owner, abilityData, availableBonuses)
    {
    }

    public override void End(Vector3 target)
    {
        // interpret target
        Vector3 ownerXZPosition = Owner.transform.position;
        ownerXZPosition.y = 0;
        Vector3 targetXZPosition = target;
        targetXZPosition.y = 0;

        float distance = Vector3.Distance(ownerXZPosition, targetXZPosition);
        distance = Mathf.Clamp(distance, minimumThrowDistance, maximumThrowDistance);

        float throwHeightOffset = target.y - Owner.transform.position.y - MouseGamePositionFinder.Instance.HeightOffset;

        float throwAngle =
            Mathf.Atan(
                (Mathf.Pow(throwVelocity, 2)
                - Mathf.Sqrt(
                    Mathf.Pow(throwVelocity, 4)
                    - gravity * (gravity * Mathf.Pow(distance, 2) + 2 * throwHeightOffset * Mathf.Pow(throwVelocity, 2))
                    )
                ) / (gravity * distance)
            );

        if (float.IsNaN(throwAngle)) throwAngle = Mathf.PI / 4; // when target is not physically reachable, fire for max range.

        float projectileTime = distance / (throwVelocity * Mathf.Cos(throwAngle));

        // instantiate a net throw object
        Owner.MovementManager.LookAt(target);

        NetThrowObject netThrowObject = NetThrowObject.Create(Owner, throwVelocity, throwAngle, projectileTime);

        // handle rooting of enemies & success feedback, remember to do root from where the netThrowObject is after the projectile time period.
        SuccessFeedbackSubject.Next(true);
    }
}
