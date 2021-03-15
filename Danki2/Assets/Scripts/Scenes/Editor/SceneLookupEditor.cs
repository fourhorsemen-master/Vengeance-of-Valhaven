using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneLookup))]
public class SceneLookupEditor : Editor
{
    private readonly EnumDictionary<Scene, bool> foldoutStatus = new EnumDictionary<Scene, bool>(false);
    
    public override void OnInspectorGUI()
    {
        SceneLookup sceneLookup = (SceneLookup) target;

        EditorUtils.ShowScriptLink(sceneLookup);

        EnumUtils.ForEach<Scene>(scene =>
        {
            foldoutStatus[scene] = EditorGUILayout.Foldout(foldoutStatus[scene], scene.ToString());
            if (!foldoutStatus[scene]) return;

            EditorGUI.indentLevel++;

            EditSceneData(sceneLookup.sceneDataLookup[scene]);

            EditorGUI.indentLevel--;
        });

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditSceneData(SceneData sceneData)
    {
        sceneData.FileName = EditorGUILayout.TextField("File name", sceneData.FileName);
        sceneData.SceneType = (SceneType) EditorGUILayout.EnumPopup("Scene type", sceneData.SceneType);

        if (sceneData.SceneType == SceneType.Gameplay) EditGameplaySceneData(sceneData.GameplaySceneData);
    }

    private void EditGameplaySceneData(GameplaySceneData gameplaySceneData)
    {
        EditorUtils.Header("Camera Orientations");
        EditorGUI.indentLevel++;

        List<Pole> cameraOrientations = gameplaySceneData.CameraOrientations;

        for (int i = 0; i < cameraOrientations.Count; i++)
        {
            cameraOrientations[i] = (Pole) EditorGUILayout.EnumPopup("Orientation", cameraOrientations[i]);
        }

        EditorUtils.EditListSize("Add Orientation", "Remove Orientation", cameraOrientations, Pole.North);

        EditorGUI.indentLevel--;
    }
}
