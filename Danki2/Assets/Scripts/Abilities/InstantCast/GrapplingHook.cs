using UnityEngine;

[Ability(AbilityReference.GrapplingHook)]
public class GrapplingHook : InstantCast
{
    private const float range = 10f;
    private const float hookSpeed = 16f;
    private const float pullSpeed = 8f;
    private const float pullOffset = 2f;
    private const float stunDuration = 2f;

    public GrapplingHook(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Quaternion rotation = Quaternion.LookRotation(offsetTargetPosition - Owner.Centre);

        Owner.MovementManager.LookAt(offsetTargetPosition);
        Owner.MovementManager.Pause(range / hookSpeed);

        GrapplingHookObject.Fire(Owner, OnCollision, hookSpeed, Owner.AbilitySource, rotation, range);
    }

    private void OnCollision(GameObject gameObject)
    {
        if (ActorCache.Instance.TryGetActor(gameObject, out Actor actor))
        {
            if (!actor.Opposes(Owner)) return;
            
            float distanceFromOwner = Vector3.Distance(actor.transform.position, Owner.transform.position);

            float pullDuration = (distanceFromOwner - pullOffset) / pullSpeed;
            Vector3 pullDirection = Owner.transform.position - actor.transform.position;
            Vector3 pullFaceDirection = actor.transform.forward;

            Owner.MovementManager.Pause(pullDuration);

            actor.MovementManager.TryLockMovement(
                MovementLockType.Pull,
                pullDuration,
                pullSpeed,
                pullDirection,
                pullFaceDirection
            );

            actor.WaitAndAct(pullDuration, () => actor.EffectManager.AddActiveEffect(ActiveEffect.Stun, stunDuration));

            CustomCamera.Instance.AddShake(ShakeIntensity.Low);
        }

    }
}
