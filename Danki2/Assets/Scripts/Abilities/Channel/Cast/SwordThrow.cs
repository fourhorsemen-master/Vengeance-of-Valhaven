using UnityEngine;

[Ability(AbilityReference.SwordThrow, new[] { "Poison Sword" })]
public class SwordThrow : Cast
{
    private const float swordSpeed = 10f;
    private const float poisonSwordDOTLength = 5f;

    public override ChannelEffectOnMovement EffectOnMovement => ChannelEffectOnMovement.Root;

    public SwordThrow(Actor owner, AbilityData abilityData, string[] availableBonuses, float duration)
        : base(owner, abilityData, availableBonuses, duration)
    {
    }

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        CustomCamera.Instance.AddShake(ShakeIntensity.Low);

        Quaternion rotation = Quaternion.LookRotation(offsetTargetPosition - Owner.Centre);
        SwordThrowObject.Fire(Owner, OnCollision, swordSpeed, Owner.Centre, rotation);
    }

    private void OnCollision(GameObject gameObject)
    {
        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);

        if (RoomManager.Instance.TryGetActor(gameObject, out Actor actor))
        {
            if (!actor.Opposes(Owner))
            {
                SuccessFeedbackSubject.Next(false);
                return;
            }

            DealPrimaryDamage(actor);
            if (HasBonus("Poison Sword")) ApplySecondaryDamageAsDOT(actor, poisonSwordDOTLength);

            SuccessFeedbackSubject.Next(true);
        }
        else
        {            
            SuccessFeedbackSubject.Next(false);
        }
    }
}
