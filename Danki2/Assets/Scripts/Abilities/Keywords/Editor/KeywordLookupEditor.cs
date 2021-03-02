using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KeywordLookup))]
public class KeywordLookupEditor : Editor
{
    private readonly EnumDictionary<Keyword, bool> foldoutStatus = new EnumDictionary<Keyword, bool> (false);

    public override void OnInspectorGUI()
    {
        KeywordLookup keywordLookup = (KeywordLookup)target;

        EnumUtils.ForEach<Keyword>(keyword =>
        {
            var keywordData = keywordLookup.keywordLookup[keyword];

            var missingData = string.IsNullOrEmpty(keywordData.DisplayName) || string.IsNullOrEmpty(keywordData.Description);

            foldoutStatus[keyword] = EditorGUILayout.Foldout(
                foldoutStatus[keyword],
                missingData ? $"{keyword.ToString()}*" : keyword.ToString()
            );

            if (!foldoutStatus[keyword]) return;

            EditorGUI.indentLevel++;
            keywordData.DisplayName = EditorGUILayout.TextField("Display Name", keywordData.DisplayName);
            keywordData.Description = EditorGUILayout.TextField("Description", keywordData.Description);
            EditorGUI.indentLevel--;
        });

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}