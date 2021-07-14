using UnityEngine;

[Ability(AbilityReference.RingOfDeath, new [] { "Double Down", "Barbed Daggers" })]
public class RingOfDeath : InstantCast
{
    private const int BaseNumberOfKnives = 12;
    private const int DoubleDownNumberOfKnives = 24;
    private const float KnifeArcAngle = 360f;
    private const float KnifeSpeed = 10f;
    private const float BaseKnifeCastInterval = 0.04f;
    private const float DrawTime = 0.08f;

    private int NumberOfKnives => HasBonus("Double Down") ? DoubleDownNumberOfKnives : BaseNumberOfKnives;
    private float KnifeCastInterval => HasBonus("Double Down") ? BaseKnifeCastInterval / 2 : BaseKnifeCastInterval;

    public RingOfDeath(AbilityConstructionArgs arguments) : base(arguments) { }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);

        Quaternion rotation = Owner.transform.rotation;

        Owner.MovementManager.Pause(DrawTime + KnifeCastInterval * NumberOfKnives);

        PlayVocalisationEvent();
        PlayStartEvent();

        Owner.InterruptibleIntervalAction(
            KnifeCastInterval,
            InterruptionType.Hard,
            index => Throw(rotation, index),
            DrawTime,
            NumberOfKnives
        );
    }

    private void Throw(Quaternion rotation, int index)
    {
        PlayEndEvent();

        float angleOffset = KnifeArcAngle * index / NumberOfKnives;

        Quaternion castRotation = rotation * Quaternion.Euler(Vector3.up * angleOffset);

        if (HasBonus("Barbed Daggers")) 
        {
            BarbedDaggerObject.Fire(Owner, OnCollision, KnifeSpeed, Owner.AbilitySource, castRotation);
        }
        else
        {
            FanOfKnivesObject.Fire(Owner, OnCollision, KnifeSpeed, Owner.AbilitySource, castRotation);
        }
    }

    private void OnCollision(GameObject gameObject)
    {
        if (ActorCache.Instance.TryGetActor(gameObject, out Actor actor))
        {
            if (actor.Opposes(Owner))
            {
                CustomCamera.Instance.AddShake(ShakeIntensity.Low);
                DealPrimaryDamage(actor);
                if (HasBonus("Barbed Daggers")) actor.EffectManager.AddStack(StackingEffect.Bleed);
            }
        }
    }
}
