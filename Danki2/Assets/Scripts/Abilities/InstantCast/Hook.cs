using UnityEngine;

[Ability(AbilityReference.Hook)]
public class Hook : InstantCast
{
    private const float range = 10f;
    private const float hookSpeed = 16f;
    private const float pullSpeed = 8f;
    private const float pullOffset = 2f;
    private const float stunDuration = 2f;

    private HookObject hookObject = null;

    public Hook(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Quaternion rotation = Quaternion.LookRotation(offsetTargetPosition - Owner.Centre);

        Owner.MovementManager.LookAt(offsetTargetPosition);
        Owner.MovementManager.Pause(range / hookSpeed);

        hookObject = HookObject.Fire(Owner, OnCollision, MissCallback, hookSpeed, Owner.Centre, rotation, range);
    }

    private void MissCallback()
    {
        SuccessFeedbackSubject.Next(false);
    }

    private void OnCollision(GameObject gameObject)
    {
        hookObject.PlayHitAudio();

        if (RoomManager.Instance.TryGetActor(gameObject, out Actor actor))
        {
            if (!actor.Opposes(Owner))
            {
                SuccessFeedbackSubject.Next(false);
                return;
            }
            
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

            actor.WaitAndAct(pullDuration, () => actor.EffectManager.AddActiveEffect(new Stun(), stunDuration));

            SuccessFeedbackSubject.Next(true);

            CustomCamera.Instance.AddShake(ShakeIntensity.Low);
        }
        else
        {
            SuccessFeedbackSubject.Next(false);
        }

    }
}
