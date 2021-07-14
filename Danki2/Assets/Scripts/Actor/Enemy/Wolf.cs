using UnityEngine;

public class Wolf : Enemy
{
    [Header("Dash")]
    [SerializeField] private float dashDuration = 0f;
    [SerializeField] private float dashSpeed = 0f;
    
    [Header("Bite")]
    [SerializeField] private int biteDamage = 0;
    [SerializeField] private float biteRange = 0f;
    [SerializeField] private float bitePauseDuration = 0f;

    [Header("Pounce")]
    [SerializeField] private int pounceDamage = 0;
    [SerializeField] private float pounceMinMovementDuration = 0f;
    [SerializeField] private float pounceMaxMovementDuration = 0f;
    [SerializeField] private float pounceSpeedMultiplier = 0f;
    [SerializeField] private float pounceDamageRage = 0f;
    [SerializeField] private float pouncePauseDuration = 0f;

    public override ActorType Type => ActorType.Wolf;

    public Subject OnHowl { get; } = new Subject();
    public Subject OnBite { get; } = new Subject();
    public float DashDuration => dashDuration;

    public void Dash(Vector3 direction)
    {
        MovementManager.LockMovement(dashDuration, dashSpeed, direction, direction);
    }

    public void Bite()
    {
        Vector3 forward = transform.forward;
        Quaternion castRotation = AbilityUtils.GetMeleeCastRotation(forward);

        BiteObject.Create(AbilitySource, castRotation);

        MovementManager.LookAt(transform.position + forward);
        MovementManager.Pause(bitePauseDuration);

        AbilityUtils.TemplateCollision(
            this,
            CollisionTemplateShape.Wedge45,
            biteRange,
            CollisionTemplateSource,
            castRotation,
            actor =>
            {
                actor.HealthManager.ReceiveDamage(biteDamage, this);
                CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            }
        );

        OnBite.Next();
    }

    public void Pounce(Vector3 targetPosition)
    {
        Vector3 position = transform.position;
        Vector3 direction = targetPosition - position;

        float distance = Vector3.Distance(targetPosition, position);
        float pounceSpeed = StatsManager.Get(Stat.Speed) * pounceSpeedMultiplier;
        float duration = Mathf.Clamp(distance / pounceSpeed, pounceMinMovementDuration, pounceMaxMovementDuration);

        MovementManager.LockMovement(duration, pounceSpeed, direction, direction);
        StartTrail(duration);

        InterruptibleAction(duration, InterruptionType.Hard, HandlePounceLand);
    }

    private void HandlePounceLand()
    {
        Quaternion castRotation = Quaternion.LookRotation(transform.forward);
        
        AbilityUtils.TemplateCollision(
            this,
            CollisionTemplateShape.Wedge90,
            pounceDamageRage,
            CollisionTemplateSource,
            castRotation,
            actor =>
            {
                actor.HealthManager.ReceiveDamage(pounceDamage, this);
                CustomCamera.Instance.AddShake(ShakeIntensity.Medium);
            }
        );

        BiteObject.Create(AbilitySource, castRotation);

        MovementManager.Pause(pouncePauseDuration);
    }

    public void Howl()
    {
        // FMOD_TODO: play howl event here
        OnHowl.Next();
    }
}
