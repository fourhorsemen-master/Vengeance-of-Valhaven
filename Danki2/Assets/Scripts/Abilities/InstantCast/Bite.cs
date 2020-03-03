using UnityEngine;

public class Bite : InstantCast
{
    public static readonly float Range = 2f;
    public static readonly float FinalRootDuration = 0.5f;

    public Bite(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {  
        Actor owner = Context.Owner;

        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;
        target.y = 0;

        float damage = owner.GetStat(Stat.Strength);

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
            }
        });

        BiteObject.Create(position, Quaternion.LookRotation(target - position));
        owner.Root(FinalRootDuration, owner.transform.forward);
    }
}
