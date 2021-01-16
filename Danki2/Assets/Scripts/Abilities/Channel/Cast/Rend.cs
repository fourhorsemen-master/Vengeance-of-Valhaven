using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.Rend)]
public class Rend : Cast
{
    private const float Range = 3f;
    private int bleedStacks = 2;

    public Rend(Actor owner, AbilityData abilityData, string[] availableBonuses, float duration)
        : base(owner, abilityData, availableBonuses, duration)
    {
    }

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);

        List<Actor> opposingActors = CollisionTemplateManager.Instance
            .GetCollidingActors(CollisionTemplate.Cylinder, Range, Owner.transform.position)
            .Where(Owner.Opposes);

        bool enemiesHit = opposingActors.Count > 0;
        RendObject.Create(Owner.transform, Owner.Centre, enemiesHit);

        if (!enemiesHit)
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

        SuccessFeedbackSubject.Next(true);

        opposingActors.ForEach(actor =>
        {
            actor.EffectManager.AddStacks(StackingEffect.Bleed, bleedStacks);
        });

        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }
}
