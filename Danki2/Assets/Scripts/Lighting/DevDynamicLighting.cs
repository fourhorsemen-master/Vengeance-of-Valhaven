using UnityEngine;

public class DevDynamicLighting : MonoBehaviour
{
    [SerializeField]
    private DynamicLighting dynamicLighting = null;

    [SerializeField]
    private Pole cameraOrientation = Pole.North;

    [SerializeField, Range(0, 1)]
    private float depthProportion = 0;

    public void InitialiseLights()
    {
        dynamicLighting.InitialiseLights(depthProportion, cameraOrientation);
    }
}
