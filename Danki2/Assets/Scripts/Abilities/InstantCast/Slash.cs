using System;
using UnityEngine;

public class Slash : InstantCast
{
    private const float Range = 4f;
    private const float DamageMultiplier = 1.5f;

    public Slash(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Actor owner = Context.Owner;

        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;
        Vector3 castDirection = target - position;
        castDirection.y = 0f;

        int damage = Mathf.CeilToInt(owner.GetStat(Stat.Strength) * DamageMultiplier);
        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge90,
            Range,
            position,
            Quaternion.LookRotation(castDirection)
        ).ForEach(actor =>
        {
            if (owner.Opposes(actor))
            {
                actor.ModifyHealth(-damage);
                hasDealtDamage = true;
            }
        });

        SuccessFeedbackSubject.Next(hasDealtDamage);

        GameObject.Instantiate(
            AbilityObjectPrefabLookup.Instance.SlashObjectPrefab, 
            position, 
            Quaternion.LookRotation(castDirection)
        );
    }
}
