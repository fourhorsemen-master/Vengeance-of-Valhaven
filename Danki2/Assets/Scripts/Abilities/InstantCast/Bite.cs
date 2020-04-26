using UnityEngine;
public class Bite : InstantCast
{
    public static readonly AbilityData BaseAbilityData = new AbilityData(0, 0, 0);

    public const int Damage = 5;
    public const float Range = 2f;
    private const float DelayBeforeDamage = 0.75f;
    private const float PauseDuration = 0.3f;

    public Bite(AbilityContext context, AbilityData abilityData) : base(context, abilityData)
    {
    }

    public override void Cast()
    {  
        Actor owner = Context.Owner;
        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;
        target.y = 0f;

        BiteObject.Create(owner.transform);

        owner.MovementManager.LookAt(target);
        owner.MovementManager.Stun(DelayBeforeDamage + PauseDuration);

        owner.WaitAndAct(DelayBeforeDamage, () =>
        {
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
                    actor.ModifyHealth(-Damage);
                    hasDealtDamage = true;
                    actor.InterruptionManager.Interrupt(InterruptionType.Soft);
                }
            });

            SuccessFeedbackSubject.Next(hasDealtDamage);

            if (hasDealtDamage)
            {
                CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            }
        });

    }
}
