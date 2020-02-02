using UnityEngine;

public class Wolf : Enemy
{
    public override ActorType Type => ActorType.Wolf;

    protected override void OnDeath()
    {
        Debug.Log("Wolf died");
    }
}
