using UnityEngine;

public class Wolf : Enemy
{
    [SerializeField]
    private AudioSource howl = null;

    public override ActorType Type => ActorType.Wolf;

    public Subject OnHowl { get; } = new Subject();
    public Subject OnAttack { get; } = new Subject();

    public void Bite()
    {
        bool hasCast = InstantCastService.TryCast(
            AbilityReference.Bite,
            GetBiteTargetPosition(transform.position),
            GetBiteTargetPosition(Centre)
        );
        
        if (hasCast) OnAttack.Next();
    }

    public void Pounce(Actor target)
    {
        bool hasCast = InstantCastService.TryCast(
            AbilityReference.Pounce,
            GetPounceTargetPosition(transform.position, target.transform.position),
            GetPounceTargetPosition(Centre, target.Centre)
        );

        if (hasCast) OnAttack.Next();
    }

    public void Howl()
    {
        howl.Play();
        OnHowl.Next();
    }

    private Vector3 GetBiteTargetPosition(Vector3 origin) => origin + transform.forward;

    private Vector3 GetPounceTargetPosition(Vector3 origin, Vector3 target) => target + (origin - target).normalized;
}
