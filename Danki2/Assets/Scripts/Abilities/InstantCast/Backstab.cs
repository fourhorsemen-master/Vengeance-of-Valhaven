using UnityEngine;

[Ability(AbilityReference.Backstab)]
public class Backstab : InstantCast
{
    private const float Range = 4f;
    private const float PauseDuration = 0.3f;

    public Backstab(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        SuccessFeedbackSubject.Next(false);
    }

    public override void Cast(Actor target)
    {
        if (
            !Owner.Opposes(target)
            || Range < Vector3.Distance(target.transform.position, Owner.transform.position)
        )
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            Range,
            target.transform.forward,
            target.transform.rotation
        ).ForEach(actor =>
        {
            if (target.Opposes(actor))
            {
                SuccessFeedbackSubject.Next(false);
                return;
            }
        });

        DealPrimaryDamage(target);
        SuccessFeedbackSubject.Next(true);

        Vector3 castDirection = target.transform.position - Owner.transform.position;
        castDirection.y = 0f;
        BackstabObject backstabObject = BackstabObject.Create(Owner.transform.position, Quaternion.LookRotation(castDirection));

        Owner.MovementManager.LookAt(target.transform.position);
        Owner.MovementManager.Stun(PauseDuration);

        CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        backstabObject.PlayHitSound();
    }
}