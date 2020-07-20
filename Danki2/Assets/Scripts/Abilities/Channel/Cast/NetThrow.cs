using UnityEngine;

[Ability(AbilityReference.NetThrow)]
public class NetThrow : Cast
{
    protected override float CastTime => 2f;

    private const float minimumThrowDistance = 3f;
    private const float maximumThrowDistance = 15f;

    private float gravity => Physics.gravity.magnitude;
    private float throwVelocity => Mathf.Sqrt(Mathf.Pow(maximumThrowDistance, 2) / gravity); // R,max = V^2/g

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

        float throwHeightOffset = target.y - Owner.transform.position.y;

        float throwAngle =
            Mathf.Atan(
                Mathf.Pow(throwVelocity, 2) / (gravity * distance)
                + Mathf.Sqrt(
                    Mathf.Pow(throwVelocity, 4)
                    - gravity * (gravity * Mathf.Pow(distance, 2) + 2 * throwHeightOffset * Mathf.Pow(throwVelocity, 2))
                    )
                );

        float projectileTime = distance / (throwVelocity * Mathf.Cos(throwAngle));

        // instantiate a net throw object
        Owner.transform.LookAt(target, Owner.transform.up);

        NetThrowObject netThrowObject = NetThrowObject.Create()

        // handle rooting of enemies & success feedback
        SuccessFeedbackSubject.Next(true);
    }
}
