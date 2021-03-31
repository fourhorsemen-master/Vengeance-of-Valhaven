using UnityEngine;

public class RandomTransformer : MonoBehaviour
{
    [SerializeField] private PerlinNoiseEmitter2D perlinNoiseEmitter = null;

    private float initialX;
    private float initialZ;
    
    private void Start()
    {
        Vector3 position = transform.position;
        initialX = position.x;
        initialZ = position.z;
    }

    private void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = initialX + perlinNoiseEmitter.Value1;
        newPosition.z = initialZ + perlinNoiseEmitter.Value2;
        transform.position = newPosition;
    }
}
