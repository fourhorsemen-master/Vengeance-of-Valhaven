using UnityEngine;
using UnityEditor;
using System;

public class ActorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Actor actor = (Actor)target;

        EditorUtils.Header("Stats");
        EditorGUI.indentLevel++;
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            actor.baseStats[stat] = EditorGUILayout.IntSlider(stat.ToString(), actor.baseStats[stat], 0, 100);
        }
        EditorGUI.indentLevel--;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
