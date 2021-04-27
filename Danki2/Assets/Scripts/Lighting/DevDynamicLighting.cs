using UnityEngine;

public class DevDynamicLighting : MonoBehaviour
{
    [SerializeField]
    private DynamicLighting dynamicLighting = null;

    [SerializeField, Range(0, 1)]
    private float depthProportion = 0;
    
    private void Start()
    {
        Destroy(this);
    }

    public void InitialiseLights()
    {
        dynamicLighting.InitialiseLights(depthProportion);
    }
}
