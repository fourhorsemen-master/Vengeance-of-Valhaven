using UnityEngine;

public class DevDynamicLighting : MonoBehaviour
{
    [SerializeField] public DynamicLighting dynamicLighting = null;
    [SerializeField] public float depthProportion = 0;
    
    private void Start()
    {
        Destroy(this);
    }

    public void InitialiseLights()
    {
        dynamicLighting.InitialiseLights(depthProportion);
    }
}
