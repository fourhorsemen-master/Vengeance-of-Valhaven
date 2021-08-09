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

        SetupGeneralOptions(ambientEventsManager);

        EditorUtils.VerticalSpace();

        SetupAmbientEventsList(ambientEventsManager);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void SetupGeneralOptions(AmbientEventsManager ambientEventsManager)
    {
        EditorUtils.Header("General Options");

        EditorUtils.VerticalSpace();

        ambientEventsManager.MinInterval = EditorGUILayout.FloatField(new GUIContent("Min Interval"), ambientEventsManager.MinInterval);
        ambientEventsManager.MaxInterval = EditorGUILayout.FloatField(new GUIContent("Max Interval"), ambientEventsManager.MaxInterval);
        ambientEventsManager.MinDistanceFromPlayer = EditorGUILayout.FloatField(new GUIContent("Minimum Distance"), ambientEventsManager.MinDistanceFromPlayer);
        ambientEventsManager.MaxDistanceFromPlayer = EditorGUILayout.FloatField(new GUIContent("Maximum Distance"), ambientEventsManager.MaxDistanceFromPlayer);
    }

    private void SetupAmbientEventsList(AmbientEventsManager ambientEventsManager)
    {
        EditorUtils.Header("Ambient Events");

        EditorUtils.ResizeableList(
            ambientEventsManager.AmbientEvents,
            ambientEvent => EditAmbientEventData(ambientEventsManager.AmbientEvents, ambientEvent),
            ""
        );
    }

    private void EditAmbientEventData(List<string> items, string item)
    {
        EditorUtils.VerticalSpace();

        int index = items.IndexOf(item);
        SerializedProperty ambientEvents = serializedObject.FindProperty("AmbientEvents").GetArrayElementAtIndex(index);
        EditorGUILayout.PropertyField(ambientEvents, new GUIContent("Event Audio File"));
    }
}
