using UnityEngine;
using UnityEditor;
using System;

public class ActorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Actor actor = (Actor)target;

        EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            actor.stats[stat] = EditorGUILayout.IntSlider(stat.ToString(), actor.stats[stat], 0, 100);
        }
        EditorGUI.indentLevel--;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
