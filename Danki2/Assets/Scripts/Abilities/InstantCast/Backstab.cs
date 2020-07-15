using UnityEngine;

[Ability(AbilityReference.Backstab)]
public class Backstab : InstantCast
{
    private const float Range = 4f;
    private const float PauseDuration = 0.3f;

    public Backstab(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Stun(PauseDuration);
        SuccessFeedbackSubject.Next(false);
    }

    public override void Cast(Actor target)
    {
        Owner.MovementManager.LookAt(target.transform.position);
        Owner.MovementManager.Stun(PauseDuration);

        if (
            !Owner.Opposes(target)
            || Range < Vector3.Distance(target.transform.position, Owner.transform.position)
            || Vector3.Dot(target.transform.forward, Owner.transform.position - target.transform.position) > 0
        )
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

        DealPrimaryDamage(target);
        SuccessFeedbackSubject.Next(true);

        Vector3 castDirection = target.Centre - Owner.Centre;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        BackstabObject backstabObject = BackstabObject.Create(Owner.transform.position, castRotation);

        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        backstabObject.PlayHitSound();
    }
}