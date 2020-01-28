using UnityEngine;

public class Wolf : Enemy
{
    public override ActorType Type => ActorType.Wolf;

    protected override void OnDeath()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 2;
        transform.Rotate(Vector3.forward, 90f);
    }
}
