using UnityEngine;

public class Wolf : Enemy
{
    [Header("Dash")]
    [SerializeField] private float dashDuration = 0f;
    [SerializeField] private float dashSpeed = 0f;

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
        InstantCastService.TryCast(
            AbilityReference.Bite,
            GetBiteTargetPosition(transform.position),
            GetBiteTargetPosition(Centre)
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

    private Vector3 GetBiteTargetPosition(Vector3 origin) => origin + transform.forward;

    private Vector3 GetPounceTargetPosition(Vector3 origin, Vector3 target) => target + (origin - target).normalized;
}
