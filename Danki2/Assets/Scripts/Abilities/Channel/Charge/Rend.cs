using UnityEngine;

[Ability(AbilityReference.Rend)]
public class Rend : Charge
{
    private const int DamageMultiplier = 3;
    private const float Range = 3f;
    private const float DotDuration = 3f;

    private int charges = 0;
    
    protected override float ChargeTime => 3f;

    public Rend(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    protected override void Continue()
    {
        int newCharges = Mathf.FloorToInt(TimeCharged);

        if (charges == newCharges) return;

        charges = newCharges;
        CustomCamera.Instance.AddShake(ShakeIntensity.Low);
        
        if (charges == 1) SuccessFeedbackSubject.Next(true);
    }

    public override void Cancel(Vector3 target) => End();

    public override void End(Vector3 target) => End();

    private void End()
    {
        if (charges == 0)
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

        int totalDamage = charges * DamageMultiplier;
        
        CollisionTemplateManager.Instance
            .GetCollidingActors(CollisionTemplate.Cylinder, Range, Owner.transform.position)
            .Where(Owner.Opposes)
            .ForEach(actor =>
            {
                actor.EffectManager.AddActiveEffect(new DOT(totalDamage, DotDuration), DotDuration);
            });
        
        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }
}
