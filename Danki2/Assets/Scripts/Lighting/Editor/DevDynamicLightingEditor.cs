using UnityEditor;

[CustomEditor(typeof(DevDynamicLighting))]
public class DevDynamicLightingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty depthProportion = serializedObject.FindProperty("depthProportion");
        float previousDepthProportion = depthProportion.floatValue;
        
        base.OnInspectorGUI();

        if (previousDepthProportion != depthProportion.floatValue)
        {
            DevDynamicLighting devDynamicLighting = (DevDynamicLighting) target;
            devDynamicLighting.InitialiseLights();
        }
    }
}
