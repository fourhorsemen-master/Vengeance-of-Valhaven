using UnityEngine;

public class Smash : InstantCast
{
    public static readonly AbilityData BaseAbilityData = new AbilityData(0, 0, 0);

    private const float DistanceFromCaster = 1f;
    private const float Radius = 1f;
    private const int Damage = 10;

    public Smash(Actor owner, AbilityData abilityData) : base(owner, abilityData)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        target.y = 0;

        Vector3 directionToTarget = target == position ? Vector3.right : (target - position).normalized;
        Vector3 center = position + (directionToTarget * DistanceFromCaster);

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Cylinder,
            Radius,
            center
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                Owner.DamageTarget(actor, Damage);
                hasDealtDamage = true;
            }
        });

        CustomCamera.Instance.AddShake(ShakeIntensity.High);
        SmashObject.Create(position, Quaternion.LookRotation(target - position));

        SuccessFeedbackSubject.Next(hasDealtDamage);
    }
}
