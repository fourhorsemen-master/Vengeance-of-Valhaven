using UnityEngine;

class DaggerThrow : InstantCast
{
    private static readonly float daggerSpeed = 20f;
    private static readonly float impactDamageMultiplier = 0.5f;
    private static readonly float damagePerTickMultiplier = 0.5f;
    private static readonly float damageTickInterval = 1f;
    private static readonly float dotDuration = 3f;
    private static readonly Vector3 positionTransform = new Vector3(0, 1.25f, 0);

    public DaggerThrow(AbilityContext context) : base(context)
    {

    }

    public override void Cast()
    {
        Vector3 position = Context.Owner.transform.position + positionTransform;
        Vector3 target = Context.TargetPosition;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        DaggerObject.Fire(Context.Owner, OnCollision, daggerSpeed, position, rotation);
    }

    protected void OnCollision(GameObject gameObject)
    {
        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Context.Owner))
            {
                return;
            }

            int strength = Context.Owner.GetStat(Stat.Strength);

            float impactDamage = strength * impactDamageMultiplier;
            actor.ModifyHealth(-impactDamage);

            float damagePerTick = strength * damagePerTickMultiplier;
            actor.AddActiveEffect(new DOT(damagePerTick, damageTickInterval), dotDuration);
        }
    }
}
