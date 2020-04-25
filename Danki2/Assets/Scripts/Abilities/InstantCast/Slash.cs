using UnityEngine;

public class Slash : InstantCast
{
    private const float Range = 4f;
    private const float DamageMultiplier = 1.5f;
    private const float PauseDuration = 0.3f;

    public Slash(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Actor owner = Context.Owner;

        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;
        target.y = 0;

        int damage = Mathf.CeilToInt(owner.GetStat(Stat.Strength) * DamageMultiplier);
        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            Range,
            position,
            Quaternion.LookRotation(target - position)
        ).ForEach(actor =>
        {
            if (owner.Opposes(actor))
            {
                actor.ModifyHealth(-damage);
                hasDealtDamage = true;
            }
        });

        SuccessFeedbackSubject.Next(hasDealtDamage);

        SlashObject slashObject = SlashObject.Create(position, Quaternion.LookRotation(target - position));

        owner.MovementManager.LookAt(target);
        owner.MovementManager.Stun(PauseDuration);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            slashObject.PlayHitSound();
        }
    }
}
