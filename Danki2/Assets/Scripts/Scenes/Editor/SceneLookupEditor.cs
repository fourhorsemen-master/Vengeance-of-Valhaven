using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneLookup))]
public class SceneLookupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SceneLookup sceneLookup = (SceneLookup) target;

        EditorUtils.ShowScriptLink(sceneLookup);

        EnumUtils.ForEach<Scene>(scene =>
        {
            EditorUtils.Header(scene.ToString());
            EditorGUI.indentLevel++;

            SceneData sceneData = sceneLookup.sceneDataLookup[scene];
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
