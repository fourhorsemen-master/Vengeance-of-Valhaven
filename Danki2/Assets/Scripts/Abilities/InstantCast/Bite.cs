using UnityEngine;
public class Bite : InstantCast
{
    public const float Range = 2f;
    private const float DelayBeforeDamage = 0.75f;
    private const float PauseDuration = 0.3f;

    public Bite(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {  
        Actor owner = Context.Owner;
        float damage = owner.GetStat(Stat.Strength);
        Vector3 position = owner.transform.position;
        Vector3 targetPosition = Context.TargetPosition;
        targetPosition.y = 0f;

        owner.WaitAndAct(DelayBeforeDamage, () =>
        {
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
                }
            });
        });

        BiteObject.Create(owner.transform);
        owner.MovementManager.Stun(PauseDuration);
    }
}
