using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    public float Time { get; private set; } = 0;

    private void Update()
    {
        Time += UnityEngine.Time.deltaTime;
    }
}
