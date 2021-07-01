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

    public override ActorType Type => ActorType.Wolf;

    public Subject OnHowl { get; } = new Subject();
    public Subject OnBite { get; } = new Subject();
    public float DashDuration => dashDuration;

    public void Dash(Vector3 direction)
    {
        MovementManager.TryLockMovement(MovementLockType.Dash, dashDuration, dashSpeed, direction, direction);
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

    public void Pounce(Actor target)
    {
        InstantCastService.TryCast(
            AbilityReference.Pounce,
            GetPounceTargetPosition(transform.position, target.transform.position),
            GetPounceTargetPosition(Centre, target.Centre)
        );
    }

    public void Howl()
    {
        // FMOD_TODO: play howl event here
        OnHowl.Next();
    }

    private Vector3 GetPounceTargetPosition(Vector3 origin, Vector3 target) => target + (origin - target).normalized;
}
