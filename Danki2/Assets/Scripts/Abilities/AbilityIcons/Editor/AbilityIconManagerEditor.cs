using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityIconManager))]
public class AbilityIconManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AbilityIconManager abilityIconManager = (AbilityIconManager)target;

        foreach (AbilityReference ability in Enum.GetValues(typeof(AbilityReference)))
        {
            abilityIconManager.spriteLookup[ability] = (Sprite)EditorGUILayout.ObjectField(
                ability.ToString(),
                abilityIconManager.spriteLookup[ability],
                typeof(Sprite),
                false,
                null
            );
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}