using UnityEngine;

[Ability(AbilityReference.Hamstring, new []{"Hack"})]
public class Hamstring : InstantCast
{
    private const float Range = 3;
    private const float PauseDuration = 0.3f;
    private const int DefenceDebuff = 2;
    private const float DefenceDebuffDuration = 10;
    private const int HackDamageBonus = 4;
    
    public Hamstring(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses) { }

    public override void Cast(Vector3 target)
    {
        Owner.transform.LookAt(target);
        Owner.MovementManager.Pause(PauseDuration);

        SuccessFeedbackSubject.Next(false);
    }

    public override void Cast(Actor target)
    {
        Owner.transform.LookAt(target.transform.position);
        Owner.MovementManager.Pause(PauseDuration);

        if (!InRange(target))
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

        SuccessFeedbackSubject.Next(true);
        
        Damage(target);
        ApplyDebuff(target);

        Vector3 castDirection = target.Centre - Owner.Centre;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        CustomCamera.Instance.AddShake(ShakeIntensity.High);
        HamstringObject.Create(Owner.Centre, castRotation);
    }

    private bool InRange(Actor target)
    {
        return Vector3.Distance(target.transform.position, Owner.transform.position) <= Range;
    }

    private void Damage(Actor target)
    {
        DealPrimaryDamage(target, HasBonus("Hack") ? HackDamageBonus : 0);
    }

    private void ApplyDebuff(Actor target)
    {
        target.EffectManager.AddActiveEffect(new DefenceDebuff(DefenceDebuff), DefenceDebuffDuration);
    }
}
