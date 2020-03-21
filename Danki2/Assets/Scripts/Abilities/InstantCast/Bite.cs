using UnityEngine;
using System.Collections;

public class Bite : InstantCast
{
    public static readonly float Range = 2f;
    public static readonly float FinalRootDuration = 0.5f;
    public static readonly float DelayBeforeDamage = 0.75f;

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

        IEnumerator coroutine = DelayDamage(DelayBeforeDamage, position, target, owner, damage);
        owner.StartCoroutine(coroutine);

        GameObject.Instantiate(AbilityObjectPrefabLookup.Instance.BiteObjectPrefab, owner.transform);
        owner.Root(FinalRootDuration, owner.transform.forward);
    }

    private IEnumerator DelayDamage( Vector3 position, Vector3 targetPosition, Actor owner, float damage )
    {
        yield return new WaitForSeconds(DelayBeforeDamage);

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
    }
}
