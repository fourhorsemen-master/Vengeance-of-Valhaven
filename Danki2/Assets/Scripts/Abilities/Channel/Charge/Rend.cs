using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.Rend)]
public class Rend : Charge
{
    private const float Range = 3f;
    private const float DotDuration = 5f;

    protected override float ChargeTime => 3f;

    public Rend(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cancel(Vector3 target) => End();

    public override void End(Vector3 target) => End();

    private void End()
    {
        int charges = Mathf.FloorToInt(TimeCharged);

        if (charges == 0)
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

        List<Actor> opposingActors = CollisionTemplateManager.Instance
            .GetCollidingActors(CollisionTemplate.Cylinder, Range, Owner.transform.position)
            .Where(Owner.Opposes);

        if (opposingActors.Count == 0)
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

        SuccessFeedbackSubject.Next(true);

        opposingActors.ForEach(actor =>
        {
            ApplyPrimaryDamageAsDOT(actor, DotDuration, multiplicativeDamageModifier: charges);
        });

        RendObject.Create(Owner.transform);
        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }
}
