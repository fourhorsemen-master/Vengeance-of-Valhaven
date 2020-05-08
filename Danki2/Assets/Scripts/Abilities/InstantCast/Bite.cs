using System.Collections.Generic;
using UnityEngine;

public class Bite : InstantCast
{
    public static readonly AbilityData BaseAbilityData = new AbilityData(0, 0, 0, 0);
    public static readonly Dictionary<OrbType, int> GeneratedOrbs = new Dictionary<OrbType, int>();
    public const OrbType AbilityOrbType = OrbType.Aggression;
    public const string Tooltip = "Deals {DAMAGE} damage.";
    public const string DisplayName = "Bite";

    public const int Damage = 5;
    public const float Range = 2f;
    private const float PauseDuration = 0.3f;

    public Bite(Actor owner, AbilityData abilityData) : base(owner, abilityData)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        target.y = 0f;

        BiteObject.Create(Owner.transform);

        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Stun(PauseDuration);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Wedge45,
            Range,
            position,
            Quaternion.LookRotation(target - position)
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                Owner.DamageTarget(actor, Damage);
                hasDealtDamage = true;
            }
        });

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }

        SuccessFeedbackSubject.Next(hasDealtDamage);
    }
}
