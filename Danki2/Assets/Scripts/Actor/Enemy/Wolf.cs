using UnityEngine;

public class Wolf : Enemy
{
    [SerializeField]
    private float dashDuration = 0f;

    [SerializeField]
    private float dashSpeed = 0f;

    public override ActorType Type => ActorType.Wolf;

    public Subject OnHowl { get; } = new Subject();
    public Subject OnAttack { get; } = new Subject();

    public void DashFromActor(Actor actor)
    {
        Vector3 direction = transform.position - actor.transform.position;
        MovementManager.TryLockMovement(MovementLockType.Dash, dashDuration, dashSpeed, direction, direction);
    }

    public void Bite()
    {
        InstantCastService.TryCast(
            AbilityReference.Bite,
            GetBiteTargetPosition(transform.position),
            GetBiteTargetPosition(Centre)
        );
        OnAttack.Next();
    }

    public void Pounce(Actor target)
    {
        InstantCastService.TryCast(
            AbilityReference.Pounce,
            GetPounceTargetPosition(transform.position, target.transform.position),
            GetPounceTargetPosition(Centre, target.Centre)
        );
        OnAttack.Next();
    }

    public void Howl()
    {
        // FMOD_TODO: play howl event here
        OnHowl.Next();
    }

    private Vector3 GetBiteTargetPosition(Vector3 origin) => origin + transform.forward;

    private Vector3 GetPounceTargetPosition(Vector3 origin, Vector3 target) => target + (origin - target).normalized;
}
