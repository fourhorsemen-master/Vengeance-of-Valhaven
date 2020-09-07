using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.Rend)]
public class Rend : Charge
{
    private const float Range = 3f;
    private const float DotDuration = 5f;

    private int cameraShakeCount = 0;

    protected override float ChargeTime => 3f;

    public Rend(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    protected override void Continue()
    {
        int newCameraShakeCount = Mathf.FloorToInt(TimeCharged);
        if (newCameraShakeCount == cameraShakeCount) return;
        cameraShakeCount = newCameraShakeCount;
        CustomCamera.Instance.AddShake(ShakeIntensity.Low);
    }

    public override void Cancel(Vector3 target) => End(TimeCharged);

    public override void End(Vector3 target) => End(ChargeTime);

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
            ApplyPrimaryDamageAsDOT(actor, DotDuration, multiplicativeDamageModifier: charges);
        });

        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
    }
}
