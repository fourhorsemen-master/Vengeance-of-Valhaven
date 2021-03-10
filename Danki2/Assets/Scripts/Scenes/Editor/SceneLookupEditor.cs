using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneLookup))]
public class SceneLookupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SceneDataLookup sceneDataLookup = ((SceneLookup) target).sceneDataLookup;
        
        EnumUtils.ForEach<Scene>(scene =>
        {
            EditorUtils.Header(scene.ToString());
            EditorGUI.indentLevel++;

            SceneData sceneData = sceneDataLookup[scene];
            sceneData.FileName = EditorGUILayout.TextField("File name", sceneData.FileName);
            sceneData.SceneType = (SceneType) EditorGUILayout.EnumPopup("Scene type", sceneData.SceneType);

            EditorGUI.indentLevel--;
        });

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
