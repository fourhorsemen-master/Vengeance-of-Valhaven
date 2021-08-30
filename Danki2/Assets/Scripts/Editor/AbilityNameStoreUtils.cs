using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AbilityNameStoreUtils : MonoBehaviour
{
    public static void SaveAbilityNames(TextAsset textAsset, List<string> abilityNames)
    {
        string path = AssetDatabase.GetAssetPath(textAsset);
        string text = string.Join(",", abilityNames);
        File.WriteAllText(path, text);
    }

    public static TextAsset EditAbilityNameStore(TextAsset textAsset)
    {
        return (TextAsset) EditorGUILayout.ObjectField(
            "Ability Name Store",
            textAsset,
            typeof(TextAsset),
            false
        );
    }

    public static string EditAbilityName(string label, string value, TextAsset textAsset)
    {
        string[] abilityNames = textAsset.text.Split(',');

        int currentIndex = Array.IndexOf(abilityNames, value);
        if (currentIndex == -1) currentIndex = 0;
        int newIndex = EditorGUILayout.Popup(label, currentIndex, abilityNames);
        return abilityNames[newIndex];
    }
}
