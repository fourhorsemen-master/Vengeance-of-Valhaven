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
        healthBar.actor = (Actor)EditorGUILayout.ObjectField("Actor", healthBar.actor, typeof(Actor), true, null);
    }
}
