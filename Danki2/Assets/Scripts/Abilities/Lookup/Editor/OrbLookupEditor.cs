using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OrbLookup))]
public class OrbLookupEditor : Editor
{
    private readonly EnumDictionary<OrbType, bool> foldoutStatus = new EnumDictionary<OrbType, bool>(false);

    public override void OnInspectorGUI()
    {
        OrbLookup orbLookup = (OrbLookup)target;

        foreach (OrbType orbType in Enum.GetValues(typeof(OrbType)))
        {
            foldoutStatus[orbType] = EditorGUILayout.Foldout(
                foldoutStatus[orbType],
                orbType.ToString()
            );

            if (!foldoutStatus[orbType]) continue;

            EditorGUI.indentLevel++;

            orbLookup.displayNameMap[orbType] = EditorGUILayout.TextField("Display name", orbLookup.displayNameMap[orbType]);

            orbLookup.descriptionMap[orbType] = EditorGUILayout.TextField("Description", orbLookup.descriptionMap[orbType]);

            orbLookup.colourMap[orbType] = EditorGUILayout.ColorField("Colour (text etc.)", orbLookup.colourMap[orbType]);

            orbLookup.spriteMap[orbType] = (Sprite)EditorGUILayout.ObjectField(
                "Sprite",
                orbLookup.spriteMap[orbType],
                typeof(Sprite),
                false,
                null
            );

            EditorGUI.indentLevel--;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}