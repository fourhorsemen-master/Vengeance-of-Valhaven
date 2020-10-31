using UnityEngine;

public class Wolf : Enemy
{
    [SerializeField]
    private float biteDelay = 0f;

    [SerializeField]
    private float pounceDelay = 0f;

    [SerializeField]
    private AudioSource howl;

    public override ActorType Type => ActorType.Wolf;

    public Subject OnHowl { get; } = new Subject();
    public Subject OnAttack { get; } = new Subject();

    public void Bite()
    {
        WaitAndCast(
            biteDelay,
            AbilityReference.Bite,
            () => transform.position + transform.forward,
            () => OnAttack.Next()
        );
    }

    public void Pounce()
    {
        WaitAndCast(
            pounceDelay,
            AbilityReference.Pounce,
            () => Target.transform.position + (transform.position - Target.transform.position).normalized,
            () => OnAttack.Next()
        );
    }

    public void Howl()
    {
        howl.Play();
        OnHowl.Next();
    }

    // public void GetAttention(Player player)
    // {
    //     if (attentionCoroutine != null) return;
    //
    //     MovementManager.StopPathfinding();
    //     MovementManager.Watch(player.transform);
    //     attentionCoroutine = this.WaitAndAct(agroTime, () =>
    //     {
    //         FindTarget(player);
    //     });
    // }

    // public void LoseAttention()
    // {
    //     if (attentionCoroutine != null)
    //     {
    //         StopCoroutine(attentionCoroutine);
    //         attentionCoroutine = null;
    //     }
    // }

    // public void FindTarget(Player player)
    // {
    //     if (attentionCoroutine != null)
    //     {
    //         StopCoroutine(attentionCoroutine);
    //     }
    //     
    //     Target = player;
    //     Howl();
    //     OnHowl.Next();
    // }
}
