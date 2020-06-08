using UnityEngine;

[Ability(AbilityReference.Bash)]
public class Bash : InstantCast
{
    private const float StunDuration = 1f;
    private const float Range = 3f;
    private const float PauseDuration = 0.3f;

    public Bash(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        Vector3 castDirection = target - position;
        castDirection.y = 0f;

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            Range,
            position,
            Quaternion.LookRotation(castDirection)
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                DealPrimaryDamage(actor);
                actor.EffectManager.AddActiveEffect(new Stun(StunDuration), StunDuration);
                hasDealtDamage = true;
            }
        });

        SuccessFeedbackSubject.Next(hasDealtDamage);

        BashObject.Create(position, Quaternion.LookRotation(castDirection));

        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Stun(PauseDuration);
        
        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }
}
