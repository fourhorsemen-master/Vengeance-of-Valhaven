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

    public DaggerThrow(AbilityContext context, Action<bool> completionCallback)
        : base(context, completionCallback)
    {
    }

    public override void Cast()
    {
        Vector3 position = Context.Owner.transform.position + positionTransform;
        Vector3 target = Context.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        DaggerObject.Fire(Context.Owner, OnCollision, DaggerSpeed, position, rotation);
    }

    private void OnCollision(GameObject gameObject)
    {
        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Context.Owner))
            {
                completionCallback(false);
                return;
            }

            int strength = Context.Owner.GetStat(Stat.Strength);

            int impactDamage = Mathf.CeilToInt(strength * ImpactDamageMultiplier);
            actor.ModifyHealth(-impactDamage);

            int damagePerTick = Mathf.CeilToInt(strength * DamagePerTickMultiplier);
            actor.EffectManager.AddActiveEffect(new DOT(damagePerTick, DamageTickInterval), DotDuration);

            completionCallback(true);
        }
    }
}
