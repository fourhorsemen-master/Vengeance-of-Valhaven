using System;
using UnityEngine;

public class Slash : InstantCast
{
    private const float Range = 4f;
    private const int Damage = 5;

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
                owner.DamageTarget(actor, Damage);
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
