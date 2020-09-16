using UnityEngine;
using UnityEngine.AI;

[Ability(AbilityReference.Bash)]
public class Bash : InstantCast
{
    private const float StunDuration = 1f;
    private const float Range = 3f;
    private const float PauseDuration = 0.3f;

    public Bash(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Actor target)
    {
        Vector3 castPosition = target.Centre;

        if (NavMesh.SamplePosition(castPosition, out NavMeshHit navMeshHit, Range, NavMesh.AllAreas))
        {
            castPosition = navMeshHit.position;
        }

        Cast(castPosition);
    }

    public override void Cast(Vector3 target)
    {
        Owner.MovementManager.LookAt(target);
        Owner.MovementManager.Pause(PauseDuration);

        Vector3 position = Owner.transform.position;
        Vector3 castDirection = target - Owner.Centre;
        Quaternion castRotation = GetMeleeCastRotation(castDirection);

        if (Range < Vector3.Distance(target, position))
        {
            SuccessFeedbackSubject.Next(false);
            return;
        }

        bool hasDealtDamage = false;

        CollisionTemplateManager.Instance.GetCollidingActors(CollisionTemplate.Wedge90, Range, position, castRotation)
            .Where(actor => Owner.Opposes(actor))
            .ForEach(actor =>
            {
                DealPrimaryDamage(actor);
                actor.EffectManager.AddActiveEffect(new Stun(), StunDuration);
                hasDealtDamage = true;
            });

        SuccessFeedbackSubject.Next(hasDealtDamage);

        BashObject.Create(target, hasDealtDamage);
        
        if (hasDealtDamage)
        {
            CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
        }
    }
}
