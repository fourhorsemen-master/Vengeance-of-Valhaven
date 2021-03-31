using UnityEngine;

public class PerlinNoiseEmitter : MonoBehaviour
{
    private const float MaxStartOffset = 1000;

    [SerializeField] private float speed = 0;
    [SerializeField] private float min = 0;
    [SerializeField] private float max = 0;

    private float x;

    public float Value { get; private set; }

    private void Start()
    {
        x = Random.Range(0, MaxStartOffset);
    }

    private void Update()
    {
        x += speed * Time.deltaTime;
        Value = min + (max - min) * Mathf.PerlinNoise(x, 0);
    }
}
