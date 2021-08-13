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
        ambientSoundManager.EventEmitter = (StudioEventEmitter)EditorGUILayout.ObjectField("Event Emitter", ambientSoundManager.EventEmitter, typeof(StudioEventEmitter), false);
        ambientSoundManager.DayNightCurve = EditorGUILayout.CurveField(new GUIContent("DayNight Curve"), ambientSoundManager.DayNightCurve);
    }

    private void SetupGeneralOptions(AmbientSoundManager ambientSoundManager)
    {
        EditorUtils.Header("General Options");

        EditorUtils.VerticalSpace();

        ambientSoundManager.MinInterval = EditorGUILayout.FloatField(new GUIContent("Min Interval"), ambientSoundManager.MinInterval);
        ambientSoundManager.MaxInterval = EditorGUILayout.FloatField(new GUIContent("Max Interval"), ambientSoundManager.MaxInterval);
        ambientSoundManager.MinDistanceFromPlayer = EditorGUILayout.FloatField(new GUIContent("Minimum Distance"), ambientSoundManager.MinDistanceFromPlayer);
        ambientSoundManager.MaxDistanceFromPlayer = EditorGUILayout.FloatField(new GUIContent("Maximum Distance"), ambientSoundManager.MaxDistanceFromPlayer);
    }

    private void SetupAmbientEventsList(AmbientSoundManager ambientSoundManager)
    {
        EditorUtils.Header("Ambient Events");

        EditorUtils.ResizeableList(
            ambientSoundManager.AmbientEvents,
            ambientEvent => EditAmbientEventData(ambientSoundManager.AmbientEvents, ambientEvent),
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
