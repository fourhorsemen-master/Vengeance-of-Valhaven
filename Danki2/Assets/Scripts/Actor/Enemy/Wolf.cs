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
        InstantCastService.Cast(AbilityReference.Bite, transform.position + transform.forward);
        OnAttack.Next();
    }

    public void Pounce(Vector3 target)
    {
        InstantCastService.Cast(AbilityReference.Pounce, target + (transform.position - target).normalized);
        OnAttack.Next();
    }

    public void Howl()
    {
        howl.Play();
        OnHowl.Next();
    }
}
