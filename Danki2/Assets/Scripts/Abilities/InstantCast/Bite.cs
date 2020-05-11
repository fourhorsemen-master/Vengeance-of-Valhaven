using System.Collections.Generic;
using UnityEngine;

[Ability(AbilityReference.Bite)]
public class Bite : InstantCast
{
    public const float Range = 2f;
    private const float PauseDuration = 0.3f;

    public Bite(Actor owner, AbilityData abilityData) : base(owner, abilityData)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        target.y = 0f;

        BiteObject.Create(Owner.transform);

        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Stun(PauseDuration);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge45,
            Range,
            position,
            Quaternion.LookRotation(target - position)
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                DealPrimaryDamage(actor);
                hasDealtDamage = true;
            }
        });

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }

        SuccessFeedbackSubject.Next(hasDealtDamage);
    }
}
