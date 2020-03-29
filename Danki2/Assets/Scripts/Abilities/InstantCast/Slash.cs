using UnityEngine;

public class Slash : InstantCast
{
    private const float Range = 4f;
    private const float DamageMultiplyer = 1.5f;

    public Slash(AbilityContext context) : base(context)
    {
    }

    public override void Cast()
    {
        Actor owner = Context.Owner;

        Vector3 position = owner.transform.position;
        Vector3 target = Context.TargetPosition;
        target.y = 0;

        float damage = owner.GetStat(Stat.Strength) * DamageMultiplyer;

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
