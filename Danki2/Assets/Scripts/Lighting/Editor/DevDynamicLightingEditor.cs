using UnityEditor;

[CustomEditor(typeof(DevDynamicLighting))]
public class DevDynamicLightingEditor : Editor
{
    private DevDynamicLighting devDynamicLighting;
    private SerializedProperty depthProportion;
    
    private void OnEnable()
    {
        devDynamicLighting = (DevDynamicLighting) target;
        depthProportion = serializedObject.FindProperty("depthProportion");
    }

    public override void OnInspectorGUI()
    {
        float previousDepthProportion = depthProportion.floatValue;
        
        base.OnInspectorGUI();

        if (previousDepthProportion != depthProportion.floatValue)
        {
            devDynamicLighting.InitialiseLights();
        }
    }
}
