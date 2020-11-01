using UnityEngine;

public class Wolf : Enemy
{
    [SerializeField]
    private float biteDelay = 0f;

    [SerializeField]
    private float pounceDelay = 0f;

    [SerializeField]
    private AudioSource howl = null;

    public override ActorType Type => ActorType.Wolf;

    public Subject OnHowl { get; } = new Subject();
    public Subject OnBite { get; } = new Subject();
    public Subject OnPounce { get; } = new Subject();

    public void Bite()
    {
        WaitAndCast(
            biteDelay,
            AbilityReference.Bite,
            () => transform.position + transform.forward,
            () => OnBite.Next()
        );
    }

    public void Pounce(Vector3 target)
    {
        WaitAndCast(
            pounceDelay,
            AbilityReference.Pounce,
            () => target + (transform.position - target).normalized,
            () => OnPounce.Next()
        );
    }

    public void Howl()
    {
        howl.Play();
        OnHowl.Next();
    }
}
