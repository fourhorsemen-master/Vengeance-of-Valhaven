using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.Rend)]
public class Rend : Charge
{
    private const float Range = 3f;

    private int cameraShakeCount = 0;

    public Rend(Actor owner, AbilityData abilityData, string[] availableBonuses, float duration)
        : base(owner, abilityData, availableBonuses, duration)
    {
    }

    protected override void Continue()
    {
        int newCameraShakeCount = Mathf.FloorToInt(TimeCharged);
        if (newCameraShakeCount == cameraShakeCount) return;
        cameraShakeCount = newCameraShakeCount;
        CustomCamera.Instance.AddShake(ShakeIntensity.Low);
    }

    public override void Cancel(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => End(TimeCharged);

    public override void End(Vector3 floorTargetPosition, Vector3 offsetTargetPosition) => End(Duration);

    private void End(float timeCharged)
    {
        int charges = Mathf.FloorToInt(timeCharged);

        if (charges == 0)
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

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
            actor.EffectManager.AddStacks(StackingEffect.Bleed, charges);
        });

        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }
}
