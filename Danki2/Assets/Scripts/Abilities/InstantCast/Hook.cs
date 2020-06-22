using UnityEngine;

[Ability(AbilityReference.Hook)]
public class Hook : InstantCast
{
    private const float range = 10f;
    private const float hookSpeed = 20f;
    private const float pullSpeed = 8f;
    private const float pullOffset = 2f;
    private const float stunDuration = 2f;

    private static readonly Vector3 positionTransform = new Vector3(0, 1f, 0);

    public Hook(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position + positionTransform;
        Quaternion rotation = Quaternion.LookRotation(target - position);

        Owner.MovementManager.LookAt(position);
        Owner.MovementManager.Stun(range / hookSpeed);

        HookObject.Fire(Owner, OnCollision, hookSpeed, position, rotation, range);
    }

    public override void Cast(Actor target)
    {
        Vector3 position = Owner.transform.position + positionTransform;
        Quaternion rotation = Quaternion.LookRotation(target.transform.position - Owner.transform.position);

        Owner.MovementManager.LookAt(position);
        Owner.MovementManager.Stun(range / hookSpeed);

        HookObject.Fire(Owner, OnCollision, hookSpeed, position, rotation, range);
    }

    private void OnCollision(GameObject gameObject)
    {
        Debug.Log("collision detected");
        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Owner))
            {
                Debug.Log("actor does not oppose");
                SuccessFeedbackSubject.Next(false);
                return;
            }
            
            float distanceFromOwner = Vector3.Distance(actor.transform.position, Owner.transform.position);

            float pullDuration = (distanceFromOwner - pullOffset) / pullSpeed;
            Vector3 pullDirection = Owner.transform.position - actor.transform.position;
            Vector3 pullFaceDirection = actor.transform.forward;

            Owner.MovementManager.Stun(pullDuration);

            actor.MovementManager.LockMovement(
                pullDuration,
                pullSpeed,
                pullDirection,
                pullFaceDirection
            );

            actor.WaitAndAct(pullDuration, () => actor.MovementManager.Stun(stunDuration));

            SuccessFeedbackSubject.Next(true);

            CustomCamera.Instance.AddShake(ShakeIntensity.Low);
        }
        else
        {
            Debug.Log("not hit actor");
            SuccessFeedbackSubject.Next(false);
        }

    }
}
