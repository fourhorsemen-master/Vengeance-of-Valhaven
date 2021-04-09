using UnityEngine;

public class RandomTransformer3D : MonoBehaviour
{
    [SerializeField] private PerlinNoiseEmitter3D perlinNoiseEmitter = null;

    private Vector3 initialPosition;
    private Vector3 transformVector = Vector3.zero;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        transform.position = initialPosition;
        transformVector.x = perlinNoiseEmitter.Value1;
        transformVector.y = perlinNoiseEmitter.Value2;
        transformVector.z = perlinNoiseEmitter.Value3;
        transform.Translate(transformVector, Space.Self);
    }
}
