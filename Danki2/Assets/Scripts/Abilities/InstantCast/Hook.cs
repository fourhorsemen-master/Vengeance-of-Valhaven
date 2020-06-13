using UnityEngine;

[Ability(AbilityReference.Hook)]
public class Hook : InstantCast
{
    private const float range = 10f;
    private const float pullSpeed = 8f;
    private const float pullOffset = 2f;
    private const float stunDuration = 2f;

    public Hook(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        SuccessFeedbackSubject.Next(false);
    }

    public override void Cast(Actor target)
    {
        float distanceToTarget = Vector3.Distance(target.transform.position, Owner.transform.position);

        if (distanceToTarget > range)
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

        float pullDuration = (distanceToTarget - pullOffset) / pullSpeed;
        Vector3 pullDirection = Owner.transform.position - target.transform.position;
        Vector3 pullFaceDirection = target.transform.forward;

        Owner.MovementManager.LookAt(target.transform.position);
        Owner.MovementManager.Stun(pullDuration);
        target.MovementManager.LockMovement(
            pullDuration,
            pullSpeed,
            pullDirection,
            pullFaceDirection
        );

        target.WaitAndAct(pullDuration, () => target.MovementManager.Stun(stunDuration));

        SuccessFeedbackSubject.Next(true);

        HookObject.Create(Owner.transform);

        CustomCamera.Instance.AddShake(ShakeIntensity.Low);
    }
}
