using UnityEngine;

[Ability(AbilityReference.Spine)]
public class Spine : InstantCast
{
    private const float SpineSpeed = 8f;
    private const float SlowDuration = 2f;

    public Spine(AbilityConstructionArgs arguments) : base(arguments) {}

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(offsetTargetPosition);

        Quaternion rotation = Quaternion.LookRotation(offsetTargetPosition - Owner.Centre);
        SpineObject.Fire(Owner, OnCollision, SpineSpeed, Owner.AbilitySource, rotation);
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
