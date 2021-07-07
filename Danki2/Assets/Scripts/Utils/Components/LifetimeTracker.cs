using UnityEngine;

public class LifetimeTracker : MonoBehaviour
{
    public float Lifetime { get; private set; } = 0;

    private void Update()
    {
        Lifetime += Time.deltaTime;
    }
}
