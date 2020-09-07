using UnityEngine;

[Ability(AbilityReference.Leap, new []{"Momentum"})]
public class Leap : InstantCast
{
    private const float MinMovementDuration = 0.1f;
    private const float MaxMovementDuration = 0.3f;
    private const float LeapSpeedMultiplier = 6f;

    private const float StunRange = 2f;
    private const float StunDuration = 3f;
    
    private readonly Subject leapEndSubject = new Subject();

    private bool HasMomentum => HasBonus("Momentum");

    public Leap(Actor owner, AbilityData abilityData, string[] availableBonuses) : base(owner, abilityData, availableBonuses)
    {
    }

    public override void Cast(Vector3 target)
    {
        Vector3 position = Owner.transform.position;
        Vector3 direction = target - position;
        direction.y = position.y;

        float distance = Vector3.Distance(target, position);
        float leapSpeed = Owner.GetStat(Stat.Speed) * LeapSpeedMultiplier;
        float duration = Mathf.Clamp(distance / leapSpeed, MinMovementDuration, MaxMovementDuration);


        Owner.MovementManager.TryLockMovement(MovementLockType.Dash, duration, leapSpeed, direction, direction);

        LeapObject leapObject = LeapObject.Create(Owner.transform, leapEndSubject, HasMomentum);
        Owner.StartTrail(duration);

        SuccessFeedbackSubject.Next(true);

        Owner.WaitAndAct(duration, () => End(leapObject));
    }

    private void End(LeapObject leapObject)
    {
        leapEndSubject.Next();
        
        if (HasMomentum) StunSurroundingEnemies(leapObject);
    }

    private void StunSurroundingEnemies(LeapObject leapObject)
    {
        bool stunHit = false;

        CollisionTemplateManager.Instance.GetCollidingActors(
            CollisionTemplate.Cylinder,
            StunRange,
            Owner.transform.position
        ).ForEach(actor =>
        {
            if (Owner.Opposes(actor))
            {
                stunHit = true;
                actor.EffectManager.AddActiveEffect(new Stun(), StunDuration);
            }
        });

        if (stunHit)
        {
            leapObject.PlayHitSound();
            CustomCamera.Instance.AddShake(ShakeIntensity.Low);
        } 
    }
}
