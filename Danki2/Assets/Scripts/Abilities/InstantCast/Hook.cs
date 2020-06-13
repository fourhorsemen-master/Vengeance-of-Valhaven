using UnityEngine;

[Ability(AbilityReference.Hook)]
public class Hook : InstantCast
{
    private const float range = 10f;
    private const float pauseDuration = 0.3f;
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

    public override void Cast(Actor actor)
    {
        float distanceToCaster = Vector3.Distance(actor.transform.position, Owner.transform.position);

        if (distanceToCaster > range)
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

        float pullDuration = (distanceToCaster - pullOffset) / pullSpeed;
        Vector3 pullDirection = Owner.transform.position - actor.transform.position;
        Vector3 pullFaceDirection = actor.transform.forward;

        Owner.MovementManager.Root(pullDuration);
        actor.MovementManager.LockMovement(
            pullDuration,
            pullSpeed,
            pullDirection,
            pullFaceDirection
            );

        actor.WaitAndAct(pullDuration, () => actor.MovementManager.Stun(stunDuration));

        SuccessFeedbackSubject.Next(true);

        Owner.MovementManager.LookAt(actor.transform.position);
        Owner.WaitAndAct(pullDuration, () => Owner.MovementManager.Stun(pauseDuration));

        HookObject.Create(Owner.transform);

        CustomCamera.Instance.AddShake(ShakeIntensity.Low);
    }
}
