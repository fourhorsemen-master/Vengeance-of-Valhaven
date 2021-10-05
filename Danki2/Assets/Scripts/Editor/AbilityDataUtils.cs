using System;
using UnityEditor;
using UnityEngine;

public class AbilityDataUtils : MonoBehaviour
{
    public static string EditAbilityName(string label, string value, AbilityData abilityData)
    {
        string[] abilityNames = abilityData.GetAbilityNames();

        int currentIndex = Array.IndexOf(abilityNames, value);
        if (currentIndex == -1) currentIndex = 0;
        int newIndex = EditorGUILayout.Popup(label, currentIndex, abilityNames);
        return abilityNames[newIndex];
    }
}
