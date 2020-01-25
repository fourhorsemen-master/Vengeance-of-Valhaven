using UnityEngine;

public class TargetDummy : Enemy
{
    protected override void OnDeath()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 5;
        transform.Rotate(Vector3.forward, 90f);
    }
}