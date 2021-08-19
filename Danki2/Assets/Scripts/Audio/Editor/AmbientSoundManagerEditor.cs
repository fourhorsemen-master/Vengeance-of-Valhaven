using FMODUnity;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AmbientSoundManager))]
public class AmbientSoundManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AmbientSoundManager ambientSoundManager = (AmbientSoundManager) target;

        EditorUtils.ShowScriptLink(ambientSoundManager);

        SetupAmbientSounds(ambientSoundManager);

        EditorUtils.VerticalSpace();

        SetupGeneralOptions(ambientSoundManager);

        EditorUtils.VerticalSpace();

        SetupAmbientEventsList(ambientSoundManager);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void SetupAmbientSounds(AmbientSoundManager ambientSoundManager)
    {
        ambientSoundManager.eventEmitter = (StudioEventEmitter)EditorGUILayout.ObjectField("Event Emitter", ambientSoundManager.eventEmitter, typeof(StudioEventEmitter), false);
        ambientSoundManager.dayNightCurve = EditorGUILayout.CurveField(new GUIContent("DayNight Curve"), ambientSoundManager.dayNightCurve);
    }

    private void SetupGeneralOptions(AmbientSoundManager ambientSoundManager)
    {
        EditorUtils.Header("General Options");

        EditorUtils.VerticalSpace();

        ambientSoundManager.minInterval = EditorGUILayout.FloatField(new GUIContent("Min Interval"), ambientSoundManager.minInterval);
        ambientSoundManager.maxInterval = EditorGUILayout.FloatField(new GUIContent("Max Interval"), ambientSoundManager.maxInterval);
        ambientSoundManager.minDistanceFromPlayer = EditorGUILayout.FloatField(new GUIContent("Minimum Distance"), ambientSoundManager.minDistanceFromPlayer);
        ambientSoundManager.maxDistanceFromPlayer = EditorGUILayout.FloatField(new GUIContent("Maximum Distance"), ambientSoundManager.maxDistanceFromPlayer);
    }

    private void SetupAmbientEventsList(AmbientSoundManager ambientSoundManager)
    {
        EditorUtils.Header("Ambient Events");

        EditorUtils.ResizeableList(
            ambientSoundManager.ambientEvents,
            ambientEvent => EditAmbientEventData(ambientSoundManager.ambientEvents, ambientEvent),
            ""
        );
    }

    private void EditAmbientEventData(List<string> items, string item)
    {
        EditorUtils.VerticalSpace();

        int index = items.IndexOf(item);
        SerializedProperty ambientEvents = serializedObject.FindProperty("ambientEvents").GetArrayElementAtIndex(index);
        EditorGUILayout.PropertyField(ambientEvents, new GUIContent("Event Audio File"));
    }
}
