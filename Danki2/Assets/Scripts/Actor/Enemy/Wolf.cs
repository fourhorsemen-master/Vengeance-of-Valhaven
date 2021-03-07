using UnityEngine;

public class Wolf : Enemy
{
    public override ActorType Type => ActorType.Wolf;

    public Subject OnHowl { get; } = new Subject();
    public Subject OnAttack { get; } = new Subject();

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
