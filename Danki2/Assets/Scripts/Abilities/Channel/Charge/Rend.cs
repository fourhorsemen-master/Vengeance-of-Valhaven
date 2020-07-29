using System.Linq;
using UnityEngine;

[Ability(AbilityReference.Rend)]
public class Rend : Charge
{
    private const int DamageMultiplier = 3;
    private const float Range = 3f;
    private const float DotDuration = 3f;

    private bool successful = false;
    
    protected override float ChargeTime => 3f;

    public Rend(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    protected override void Continue()
    {
        if (successful) return;
        if (TimeCharged < 1) return;

        SuccessFeedbackSubject.Next(true);
        successful = true;
    }

    public override void Cancel(Vector3 target) => End();

    public override void End(Vector3 target) => End();

    private void End()
    {
        if (!successful)
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

        int totalDamage = Mathf.FloorToInt(TimeCharged) * DamageMultiplier;
        
        CollisionTemplateManager.Instance
            .GetCollidingActors(CollisionTemplate.Cylinder, Range, Owner.transform.position)
            .Where(Owner.Opposes)
            .ForEach(actor =>
            {
                actor.EffectManager.AddActiveEffect(new DOT(totalDamage, DotDuration), DotDuration);
            });
    }
}
