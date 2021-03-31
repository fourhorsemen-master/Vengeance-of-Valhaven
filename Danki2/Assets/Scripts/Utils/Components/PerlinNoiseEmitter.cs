using UnityEngine;

public class PerlinNoiseEmitter : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    [SerializeField] private float min = 0;
    [SerializeField] private float max = 0;

    private float x = 0;

    public float Value { get; private set; }

    private void Update()
    {
        x += speed * Time.deltaTime;
        Value = min + (max - min) * Mathf.PerlinNoise(x, 0);
    }
}
