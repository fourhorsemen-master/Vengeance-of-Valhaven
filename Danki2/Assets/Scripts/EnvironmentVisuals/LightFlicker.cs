using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private Light lightSource = null;
    [SerializeField] private PerlinNoiseEmitter1D perlinNoiseEmitter = null;

    private void Update() => lightSource.intensity = perlinNoiseEmitter.Value;
}
