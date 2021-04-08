using UnityEngine;

public class RandomTransformer3D : MonoBehaviour
{
    [SerializeField] private PerlinNoiseEmitter3D perlinNoiseEmitter = null;

    private float initialX;
    private float initialY;
    private float initialZ;

    private void Start()
    {
        Vector3 position = transform.position;
        initialX = position.x;
        initialY = position.y;
        initialZ = position.z;
    }

    private void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = initialX + perlinNoiseEmitter.Value1;
        newPosition.y = initialY + perlinNoiseEmitter.Value2;
        newPosition.z = initialZ + perlinNoiseEmitter.Value3;
        transform.position = newPosition;
    }
}
