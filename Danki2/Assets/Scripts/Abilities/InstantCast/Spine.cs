using UnityEngine;

[Ability(AbilityReference.Spine)]
public class Spine : InstantCast
{
    private const float SpineSpeed = 7f;
    private const float SlowDuration = 2f;
    private const int NumberOfSpines = 3;
    private const float MaxAngle = 20f;
    private const float SpineInterval = 0.1f;
    private const float PauseDuration = 0.5f;

    public Spine(AbilityConstructionArgs arguments) : base(arguments) {}

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(offsetTargetPosition);

        Owner.MovementManager.Pause(PauseDuration);

        for (int i = 0; i < NumberOfSpines; i++)
        {
            Owner.InterruptibleAction(
                i * SpineInterval,
                InterruptionType.Hard,
                () =>
                {
                    Quaternion rotation = Quaternion.LookRotation(offsetTargetPosition - Owner.Centre);
                    rotation *= Quaternion.Euler(0f, Random.Range(-MaxAngle, MaxAngle), 0f);
                    SpineObject.Fire(Owner, OnCollision, SpineSpeed, Owner.AbilitySource, rotation);
                }
            );
        }
    }

    private void OnCollision(GameObject gameObject)
    {
        if (ActorCache.Instance.TryGetActor(gameObject, out Actor actor) && actor.Opposes(Owner))
        {
            DealPrimaryDamage(actor);
            actor.EffectManager.AddActiveEffect(ActiveEffect.Slow, SlowDuration);
            CustomCamera.Instance.AddShake(ShakeIntensity.Low);
            SuccessFeedbackSubject.Next(true);
            return;
        }
        
        SuccessFeedbackSubject.Next(false);
    }
}
