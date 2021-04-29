using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DevMapGenerationLookup))]
public class DevMapGenerationLookupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DevMapGenerationLookup devMapGenerationLookup = (DevMapGenerationLookup) target;

        EditorUtils.ShowScriptLink(devMapGenerationLookup);

        devMapGenerationLookup.RuneSockets = EditorGUILayout.IntField("Rune Sockets", devMapGenerationLookup.RuneSockets);
        devMapGenerationLookup.MaxRoomDepth = EditorGUILayout.IntField("Max Room Depth", devMapGenerationLookup.MaxRoomDepth);
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
