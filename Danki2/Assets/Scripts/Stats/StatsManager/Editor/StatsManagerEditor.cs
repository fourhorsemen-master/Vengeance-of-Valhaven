using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(StatsManager))]
public class StatsManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StatsManager statsManager = (StatsManager)target;

        EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            statsManager.baseStats[stat] = EditorGUILayout.IntSlider(stat.ToString(), statsManager.baseStats[stat], 0, 100);
        }
        EditorGUI.indentLevel--;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
