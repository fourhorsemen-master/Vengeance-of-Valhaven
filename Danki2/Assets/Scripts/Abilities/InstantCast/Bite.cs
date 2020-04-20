using System;
using UnityEngine;
public class Bite : InstantCast
{
    public const float Range = 2f;
    private const float DelayBeforeDamage = 0.75f;
    private const float PauseDuration = 0.3f;

    public Bite(AbilityContext context, Action<bool> completionCallback)
        : base(context, completionCallback)
    {
    }

    public override void Cast()
    {  
        Actor owner = Context.Owner;
        int damage = owner.GetStat(Stat.Strength);
        Vector3 position = owner.transform.position;
        Vector3 targetPosition = Context.TargetPosition;
        targetPosition.y = 0f;

        owner.WaitAndAct(DelayBeforeDamage, () =>
        {
            bool hasDealtDamage = false;

            CollisionTemplateManager.Instance.GetCollidingActors(
                CollisionTemplate.Wedge90,
                Range,
                position,
                Quaternion.LookRotation(targetPosition - position)
            ).ForEach(actor =>
            {
                if (owner.Opposes(actor))
                {
                    actor.ModifyHealth(-damage);
                    hasDealtDamage = true;
                    actor.InterruptionManager.Interrupt(InterruptionType.Soft);
                }
            });

            this.isSuccessfulCallback(hasDealtDamage);
        });

        BiteObject.Create(owner.transform);
        owner.MovementManager.Stun(PauseDuration);
    }
}
