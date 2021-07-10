using UnityEngine;

[Ability(AbilityReference.BarbedDagger)]
public class BarbedDagger : InstantCast
{
    private const float DaggerSpeed = 20f;
    private const float DrawTime = 0.08f;
    private const float PauseTime = 0.2f;

    public BarbedDagger(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.Pause(DrawTime + PauseTime);

        Owner.MovementManager.LookAt(offsetTargetPosition);

        PlayVocalisationEvent();
        PlayStartEvent();

        Quaternion rotation = Quaternion.LookRotation(offsetTargetPosition - Owner.Centre);

        Owner.InterruptibleAction(
            DrawTime,
            InterruptionType.Hard,
            () =>
            {
                PlayEndEvent();
                BarbedDaggerObject.Fire(Owner, OnCollision, DaggerSpeed, Owner.AbilitySource, rotation);
            }
        );

        CustomCamera.Instance.AddShake(ShakeIntensity.Low);
    }

    private void OnCollision(GameObject gameObject)
    {
        if (ActorCache.Instance.TryGetActor(gameObject, out Actor actor))
        {
            if (!actor.Opposes(Owner))
            {
                return;
            }

            DealPrimaryDamage(actor);
            actor.EffectManager.AddStack(StackingEffect.Bleed);
        }
    }
}
