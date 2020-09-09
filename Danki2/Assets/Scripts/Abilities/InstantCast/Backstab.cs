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
        Swing(target);
        SuccessFeedbackSubject.Next(false);
    }

    public override void Cast(Actor target)
    {
        BackstabObject backstabObject = Swing(target.Centre);

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

        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        backstabObject.PlayHitSound();
    }

    private BackstabObject Swing(Vector3 target)
    {
        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Pause(PauseDuration);

        Vector3 castDirection = target - Owner.Centre;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        BackstabObject backstabObject = BackstabObject.Create(Owner.Centre, castRotation);

        return backstabObject;
    }
}