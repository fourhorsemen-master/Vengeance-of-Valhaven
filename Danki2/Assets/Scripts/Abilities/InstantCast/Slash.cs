using System.Collections.Generic;
using UnityEngine;

public class Slash : InstantCast
{
    public static readonly AbilityData BaseAbilityData = new AbilityData(0, 0, 0);
    public static readonly Dictionary<OrbType, int> GeneratedOrbs = new Dictionary<OrbType, int>();
    public const OrbType AbilityOrbType = OrbType.Aggression;
    
    private const float Range = 4f;
    private const float PauseDuration = 0.3f;
    private const int Damage = 5;

    public Slash(Actor owner, AbilityData abilityData) : base(owner, abilityData)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
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
            if (Owner.Opposes(actor))
            {
                Owner.DamageTarget(actor, Damage);
                hasDealtDamage = true;
            }
        });

        SuccessFeedbackSubject.Next(hasDealtDamage);

        SlashObject slashObject = SlashObject.Create(position, Quaternion.LookRotation(castDirection));

        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Stun(PauseDuration);

        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            slashObject.PlayHitSound();
        }
    }
}
