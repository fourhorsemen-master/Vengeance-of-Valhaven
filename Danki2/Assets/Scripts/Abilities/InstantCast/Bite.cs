using UnityEngine;

public class Bite : InstantCast
{
    public static readonly float Range = 2f;
    private float _finalRootDuration = 0.5f;

    public Bite(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Debug.Log("Casting Bite");
        
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

        owner.Root(_finalRootDuration, target - position);
    }
}
