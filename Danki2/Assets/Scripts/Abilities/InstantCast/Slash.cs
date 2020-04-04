using UnityEngine;

public class Slash : InstantCast
{
    private const float Range = 4f;
    private const float DamageMultiplier = 1.5f;

    public override AbilityReference AbilityReference => AbilityReference.Slash;

    public override void Cast(AbilityContext context)
    {
        Actor owner = context.Owner;

        Vector3 position = owner.transform.position;
        Vector3 target = context.TargetPosition;
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
