using System.Collections.Generic;
using UnityEngine;

class DaggerThrow : InstantCast
{
    public static readonly AbilityData BaseAbilityData = new AbilityData(0, 0, 0);
    public static readonly Dictionary<OrbType, int> GeneratedOrbs = new Dictionary<OrbType, int>();
    public const OrbType AbilityOrbType = OrbType.Aggression;

    private const float DaggerSpeed = 20f;
    private const int ImpactDamage = 2;
    private const int TickDamage = 1;
    private const float DamageTickInterval = 1f;
    private const float DotDuration = 3f;
    private static readonly Vector3 positionTransform = new Vector3(0, 1.25f, 0);

    public DaggerThrow(Actor owner, AbilityData abilityData) : base(owner, abilityData)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position + positionTransform;
        Quaternion rotation = Quaternion.LookRotation(target - position);
        DaggerObject.Fire(Owner, OnCollision, DaggerSpeed, position, rotation);
    }

    private void OnCollision(GameObject gameObject)
    {
        if (gameObject.IsActor())
        {
            Actor actor = gameObject.GetComponent<Actor>();

            if (!actor.Opposes(Owner))
            {
                SuccessFeedbackSubject.Next(false);
                return;
            }

            Owner.DamageTarget(actor, ImpactDamage);
            actor.EffectManager.AddActiveEffect(new DOT(TickDamage, DamageTickInterval), DotDuration);
            SuccessFeedbackSubject.Next(true);
        }
        else
        {
            SuccessFeedbackSubject.Next(false);
        }
    }
}
