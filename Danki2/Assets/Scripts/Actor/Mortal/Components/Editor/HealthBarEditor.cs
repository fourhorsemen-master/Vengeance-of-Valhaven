using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HealthBar))]
public class HealthBarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HealthBar healthBar = (HealthBar)target;
        healthBar.mortal = (Mortal)EditorGUILayout.ObjectField("Mortal", healthBar.mortal, typeof(Mortal), true, null);
    }
}
