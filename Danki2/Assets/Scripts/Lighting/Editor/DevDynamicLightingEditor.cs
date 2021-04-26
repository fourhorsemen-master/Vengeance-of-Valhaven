using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DevDynamicLighting))]
public class DevDynamicLightingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DevDynamicLighting devDynamicLighting = (DevDynamicLighting) target;
        
        if (GUILayout.Button("Initialise Lights"))
        {
            devDynamicLighting.InitialiseLights();
        }
    }
}
