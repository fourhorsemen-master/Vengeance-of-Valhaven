using System;
using UnityEngine;

class DaggerThrow : InstantCast
{
    private const float DaggerSpeed = 20f;
    private const float ImpactDamageMultiplier = 0.5f;
    private const float DamagePerTickMultiplier = 0.5f;
    private const float DamageTickInterval = 1f;
    private const float DotDuration = 3f;
    private static readonly Vector3 positionTransform = new Vector3(0, 1.25f, 0);

    public override AbilityReference AbilityReference => AbilityReference.DaggerThrow;

    public override void Cast(AbilityContext context)
    {
        Vector3 position = context.Owner.transform.position + positionTransform;
        Vector3 target = context.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(target - position);

        DaggerObject.Fire(
            context.Owner,
            o => OnCollision(o, context),
            DaggerSpeed,
            position,
            rotation
        );
    }

    private void OnCollision(GameObject gameObject, AbilityContext context)
    {
        if (!gameObject.IsActor()) return;

        Actor actor = gameObject.GetComponent<Actor>();

        if (!actor.Opposes(context.Owner)) return;

        int strength = context.Owner.GetStat(Stat.Strength);

        float impactDamage = strength * ImpactDamageMultiplier;
        actor.ModifyHealth(-impactDamage);

        float damagePerTick = strength * DamagePerTickMultiplier;
        actor.AddActiveEffect(new DOT(damagePerTick, DamageTickInterval), DotDuration);
    }
}
