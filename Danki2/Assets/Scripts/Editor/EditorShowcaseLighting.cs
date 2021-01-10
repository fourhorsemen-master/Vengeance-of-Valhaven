using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShowcaseLighting))]
public class EditorShowcaseLighting : Editor
{
	SerializedProperty LightsProp;

	private void OnEnable()
	{
		LightsProp = serializedObject.FindProperty("Lights");
	}

	public override void OnInspectorGUI()
	{
		ShowcaseLighting SCL = (ShowcaseLighting)target;

		EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(30f));
		if (GUILayout.Button("Prev"))
		{
			SCL.PrevLight();
		}
		if (GUILayout.Button("Home"))
		{
			SCL.HomeLight();
		}
		if (GUILayout.Button("Next"))
		{
			SCL.NextLight();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.PropertyField(LightsProp, new GUIContent("Lights"));

		serializedObject.ApplyModifiedProperties();
	}
}
