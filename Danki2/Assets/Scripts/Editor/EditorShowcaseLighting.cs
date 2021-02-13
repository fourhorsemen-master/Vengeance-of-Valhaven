using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShowcaseLighting))]
public class EditorShowcaseLighting : Editor
{
	SerializedProperty MainLightsProp;
	SerializedProperty FillLightsProp;

	private void OnEnable()
	{
		MainLightsProp = serializedObject.FindProperty("MainLights");
		FillLightsProp = serializedObject.FindProperty("FillLights");
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

		EditorGUILayout.PropertyField(MainLightsProp, new GUIContent("Main Lights"));
		EditorGUILayout.PropertyField(FillLightsProp, new GUIContent("Fill Lights"));

		serializedObject.ApplyModifiedProperties();
	}
}
