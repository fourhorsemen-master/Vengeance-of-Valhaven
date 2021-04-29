using UnityEditor;

[CustomEditor(typeof(DevDynamicLighting))]
public class DevDynamicLightingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty depthProportion = serializedObject.FindProperty("depthProportion");
        float previousDepthProportion = depthProportion.floatValue;

        SerializedProperty cameraOrientation = serializedObject.FindProperty("cameraOrientation");
        int previousCameraOrientation = cameraOrientation.enumValueIndex;

        base.OnInspectorGUI();

        if (previousDepthProportion != depthProportion.floatValue || previousCameraOrientation != cameraOrientation.enumValueIndex)
        {
            DevDynamicLighting devDynamicLighting = (DevDynamicLighting) target;
            devDynamicLighting.InitialiseLights();
        }
    }
}
