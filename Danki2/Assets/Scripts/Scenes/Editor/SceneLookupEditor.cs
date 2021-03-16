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
        EditCameraOrientations(gameplaySceneData);
        EditEntranceData(gameplaySceneData.EntranceData);
        EditExitData(gameplaySceneData.ExitData);
        EditEnemySpawnerIds(gameplaySceneData.EnemySpawnerIds);
    }

    private void EditCameraOrientations(GameplaySceneData gameplaySceneData)
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

    private void EditEntranceData(List<EntranceData> entranceData)
    {
        EditorUtils.Header("Entrance Data");
        EditorGUI.indentLevel++;
        
        entranceData.ForEach(e =>
        {
            e.Id = EditorGUILayout.IntField("ID", e.Id);
            e.Side = (Pole) EditorGUILayout.EnumPopup("Side", e.Side);
        });

        EditorUtils.EditListSize("Add Entrance Data", "Remove Entrance Data", entranceData, () => new EntranceData());

        EditorGUI.indentLevel--;
    }

    private void EditExitData(List<ExitData> exitData)
    {
        EditorUtils.Header("Exit Data");
        EditorGUI.indentLevel++;
        
        exitData.ForEach(e =>
        {
            e.Id = EditorGUILayout.IntField("ID", e.Id);
            e.Side = (Pole) EditorGUILayout.EnumPopup("Side", e.Side);
        });

        EditorUtils.EditListSize("Add Exit Data", "Remove Exit Data", exitData, () => new ExitData());

        EditorGUI.indentLevel--;
    }

    private void EditEnemySpawnerIds(List<int> enemySpawnerIds)
    {
        EditorUtils.Header("Enemy Spawner IDs");
        EditorGUI.indentLevel++;

        for (int i = 0; i < enemySpawnerIds.Count; i++)
        {
            enemySpawnerIds[i] = EditorGUILayout.IntField("ID", enemySpawnerIds[i]);
        }

        EditorUtils.EditListSize("Add Enemy Spawner ID", "Remove Enemy Spawner ID", enemySpawnerIds, 0);

        EditorGUI.indentLevel--;
    }
}
