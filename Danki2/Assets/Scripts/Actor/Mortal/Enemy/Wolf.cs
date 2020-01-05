using UnityEngine;

public class Wolf : Enemy
{
    protected override void OnDeath()
    {
        Debug.Log("Wolf died");
    }
}
