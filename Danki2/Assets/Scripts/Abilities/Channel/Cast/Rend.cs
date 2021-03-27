using UnityEngine;

[Ability(AbilityReference.Rend)]
public class Rend : Cast
{
    private const float Range = 3f;
    private const int BleedStacks = 2;

    public Rend(Actor owner, AbilityData abilityData, string fmodStartEvent, string fmodEndEvent, string[] availableBonuses, float duration)
        : base(owner, abilityData, fmodStartEvent, fmodEndEvent, availableBonuses, duration)
    {
    }

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition)
    {
        Owner.MovementManager.LookAt(floorTargetPosition);

        bool enemiesHit = false;

        TemplateCollision(
            CollisionTemplateShape.Cylinder,
            Range,
            Owner.CollisionTemplateSource,
            Quaternion.identity,
            actor =>
            {
                actor.EffectManager.AddStacks(StackingEffect.Bleed, BleedStacks);
                enemiesHit = true;
            }
        );

        RendObject.Create(Owner.transform, Owner.AbilitySource);

        SuccessFeedbackSubject.Next(enemiesHit);

        if (enemiesHit) CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }
}
