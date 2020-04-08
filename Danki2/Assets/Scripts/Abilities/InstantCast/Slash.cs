using System;
using UnityEngine;

public class Slash : InstantCast
{
    private const float Range = 4f;
    private const float DamageMultiplier = 1.5f;

    public Slash(AbilityContext context, Action<bool> completionCallback)
        : base(context, completionCallback)
    {
    }

    public override void Cast()
    {
        Actor owner = Context.Owner;

        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;
        target.y = 0;

        float damage = owner.GetStat(Stat.Strength) * DamageMultiplier;

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

        GameObject.Instantiate(AbilityObjectPrefabLookup.Instance.SlashObjectPrefab, owner.transform);
    }
}
