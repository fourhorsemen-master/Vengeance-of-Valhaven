using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class PlayerEditor : MortalEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Player player = (Player)target;

        EditorGUILayout.LabelField("Abilities", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
            player.abilityCooldown = EditorGUILayout.Slider("Cooldown", player.abilityCooldown, 0, 2);
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Dash", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
            player.totalDashCooldown = EditorGUILayout.Slider("Cooldown", player.totalDashCooldown, 0, 5);
            player.dashDuration = EditorGUILayout.Slider("Duration", player.dashDuration, 0, 1);
            player.dashSpeedMultiplier = EditorGUILayout.Slider("Speed Multiplier", player.dashSpeedMultiplier, 1, 10);
        EditorGUI.indentLevel--;

        if (GUILayout.Button("Log Attribute Values"))
        {
            foreach (BehaviourAttribute behaviour in BehaviourScanner.GetBehaviours())
            {
                Debug.Log(behaviour.SomeValue);
                Debug.Log(behaviour.Type.Name);
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
