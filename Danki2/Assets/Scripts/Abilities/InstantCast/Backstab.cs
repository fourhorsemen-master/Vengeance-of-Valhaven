using UnityEngine;

[Ability(AbilityReference.Backstab)]
public class Backstab : InstantCast
{
    private const float Range = 4f;
    private const float PauseDuration = 0.3f;

    public Backstab(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 _, Vector3 offsetTargetPosition)
    {
        Swing(offsetTargetPosition);
        SuccessFeedbackSubject.Next(false);
    }

    public override void Cast(Actor target)
    {
        BackstabObject backstabObject = Swing(target.Centre);

        if (!InRange(target))
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

        SuccessFeedbackSubject.Next(true);

        DealPrimaryDamage(target);

        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        backstabObject.PlayHitSound();
    }

    private BackstabObject Swing(Vector3 target)
    {
        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Pause(PauseDuration);

        Vector3 castDirection = target - Owner.Centre;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        return BackstabObject.Create(Owner.Centre, castRotation);
    }

    private bool InRange(Actor target)
    {
        bool opposesCaster = Owner.Opposes(target);
        bool closeEnough = Vector3.Distance(target.transform.position, Owner.transform.position) < Range;
        bool backTurned = Vector3.Dot(target.transform.forward, Owner.transform.position - target.transform.position) < 0;

        return opposesCaster && closeEnough && backTurned;
    }
}