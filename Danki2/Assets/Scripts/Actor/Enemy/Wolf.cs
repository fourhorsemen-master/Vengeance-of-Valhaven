using UnityEngine;

public class Wolf : Enemy
{
    public AudioSource howl;

    public override ActorType Type => ActorType.Wolf;

    public void Howl()
    {
        howl.Play();
    }

    protected override void OnDeath()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 2;
        transform.Rotate(Vector3.forward, 90f);
    }
}
