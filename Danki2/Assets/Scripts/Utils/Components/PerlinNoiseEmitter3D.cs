using UnityEngine;

public class PerlinNoiseEmitter3D : MonoBehaviour
{
    private const float MaxStartOffset = 1000;

    [SerializeField] private float speed1 = 0;
    [SerializeField] private float min1 = 0;
    [SerializeField] private float max1 = 0;
    [SerializeField] private float speed2 = 0;
    [SerializeField] private float min2 = 0;
    [SerializeField] private float max2 = 0;
    [SerializeField] private float speed3 = 0;
    [SerializeField] private float min3 = 0;
    [SerializeField] private float max3 = 0;

    private float x;
    private float y;
    private float z;

    public float Value1 { get; private set; }
    public float Value2 { get; private set; }
    public float Value3 { get; private set; }

    private void Start()
    {
        x = Random.Range(0, MaxStartOffset);
        y = Random.Range(0, MaxStartOffset);
        z = Random.Range(0, MaxStartOffset);
    }

    private void Update()
    {
        x += speed1 * Time.deltaTime;
        y += speed2 * Time.deltaTime;
        z += speed3 * Time.deltaTime;
        
        Value1 = min1 + (max1 - min1) * Mathf.PerlinNoise(x, 0);
        Value2 = min2 + (max2 - min2) * Mathf.PerlinNoise(0, y);
        Value3 = min3 + (max3 - min3) * Mathf.PerlinNoise(z, z);
    }
}
