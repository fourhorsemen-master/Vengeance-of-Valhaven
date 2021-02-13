using UnityEngine;

[Ability(AbilityReference.Hamstring, new []{"Hack"})]
public class Hamstring : InstantCast
{
    private const float Range = 3;
    private const float PauseDuration = 0.3f;
    private const int VulnerableStacks = 2;
    private const int HackDamageBonus = 4;
    
    public Hamstring(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses)
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
        HamstringObject hamstringObject = Swing(target.Centre);

        if (!InRange(target))
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

        SuccessFeedbackSubject.Next(true);
        
        Damage(target);
        ApplyDebuff(target);

        CustomCamera.Instance.AddShake(ShakeIntensity.High);
    }

    private HamstringObject Swing(Vector3 target)
    {
        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Pause(PauseDuration);

        Vector3 castDirection = target - Owner.Centre;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        return HamstringObject.Create(Owner.AbilitySource, castRotation);
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
        target.EffectManager.AddStacks(StackingEffect.Vulnerable, VulnerableStacks);
    }
}
