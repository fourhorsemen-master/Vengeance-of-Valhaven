using UnityEngine;

[Ability(AbilityReference.Backstab)]
public class Backstab : InstantCast
{
    private const float Range = 3f;
    private const float PauseDuration = 0.3f;

    public Backstab(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses)
    {
    }

    public override void Cast(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
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

        bool backTurned = Vector3.Dot(target.transform.forward, Owner.transform.position - target.transform.position) < 0;

        if (backTurned)
        {
            DealPrimaryDamage(target);
        }
        else
        {
            DealSecondaryDamage(target);
        }        

        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        backstabObject.PlayHitSound();
    }

    private BackstabObject Swing(Vector3 target)
    {
        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Pause(PauseDuration);

        Vector3 castDirection = target - Owner.Centre;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        return BackstabObject.Create(Owner.AbilitySource, castRotation);
    }

    private bool InRange(Actor target)
    {
        bool opposesCaster = Owner.Opposes(target);
        bool closeEnough = Vector3.Distance(target.transform.position, Owner.transform.position) < Range;        

        return opposesCaster && closeEnough;
    }
}