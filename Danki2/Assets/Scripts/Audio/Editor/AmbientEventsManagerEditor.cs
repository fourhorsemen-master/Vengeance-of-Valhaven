using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AmbientEventsManager))]
public class AmbientEventsManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AmbientEventsManager ambientEventsManager = (AmbientEventsManager) target;

        EditorUtils.ShowScriptLink(ambientEventsManager);

        EditorUtils.Header("Ambient Events");

        EditorUtils.ResizeableList(
            ambientEventsManager.ambientEvents,
            _ => EditAmbientEventData(),
            ""
            );

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditAmbientEventData()
    {
        SerializedProperty ambientEvent = serializedObject.FindProperty("Event");
        EditorGUILayout.PropertyField(ambientEvent, new GUIContent("Event"));
    }
}
