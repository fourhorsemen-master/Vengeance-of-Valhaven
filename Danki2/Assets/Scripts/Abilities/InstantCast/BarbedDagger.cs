using UnityEngine;

[Ability(AbilityReference.BarbedDagger)]
public class BarbedDagger : InstantCast
{
    private const float DaggerSpeed = 20f;

    public BarbedDagger(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        CustomCamera.Instance.AddShake(ShakeIntensity.Low);

        Quaternion rotation = Quaternion.LookRotation(offsetTargetPosition - Owner.Centre);
        BarbedDaggerObject.Fire(Owner, OnCollision, DaggerSpeed, Owner.Centre, rotation);
    }

    private void OnCollision(GameObject gameObject)
    {
        if (RoomManager.Instance.TryGetActor(gameObject, out Actor actor))
        {
            if (!actor.Opposes(Owner))
            {
                SuccessFeedbackSubject.Next(false);
                return;
            }

            DealPrimaryDamage(actor);
            actor.EffectManager.AddStack(StackingEffect.Bleed);
            SuccessFeedbackSubject.Next(true);
        }
        else
        {
            SuccessFeedbackSubject.Next(false);
        }
    }
}
