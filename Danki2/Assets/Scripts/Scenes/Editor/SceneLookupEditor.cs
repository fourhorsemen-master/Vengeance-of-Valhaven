using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneLookup))]
public class SceneLookupEditor : Editor
{
    private readonly EnumDictionary<Scene, bool> foldoutStatus = new EnumDictionary<Scene, bool>(false);

    private readonly EnumDictionary<Scene, EnumDictionary<SceneSpecificFoldoutStatus, bool>> sceneSpecificFoldoutStatus
        = new EnumDictionary<Scene, EnumDictionary<SceneSpecificFoldoutStatus, bool>>(() =>
            new EnumDictionary<SceneSpecificFoldoutStatus, bool>(false));
    
    public override void OnInspectorGUI()
    {
        SceneLookup sceneLookup = (SceneLookup) target;

        EditorUtils.ShowScriptLink(sceneLookup);

        EnumUtils.ForEach<Scene>(scene =>
        {
            foldoutStatus[scene] = EditorGUILayout.Foldout(foldoutStatus[scene], scene.ToString());
            if (!foldoutStatus[scene]) return;

            EditorGUI.indentLevel++;

            EditSceneData(sceneLookup.sceneDataLookup[scene], sceneSpecificFoldoutStatus[scene]);

            EditorGUI.indentLevel--;
        });

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditSceneData(SceneData sceneData, EnumDictionary<SceneSpecificFoldoutStatus, bool> specificFoldoutStatus)
    {
        sceneData.FileName = EditorGUILayout.TextField("File name", sceneData.FileName);
        sceneData.SceneType = (SceneType) EditorGUILayout.EnumPopup("Scene type", sceneData.SceneType);

        if (sceneData.SceneType == SceneType.Gameplay) EditGameplaySceneData(sceneData.GameplaySceneData, specificFoldoutStatus);
    }

    private void EditGameplaySceneData(
        GameplaySceneData gameplaySceneData,
        EnumDictionary<SceneSpecificFoldoutStatus, bool> specificFoldoutStatus
    )
    {
        EditRoomTypes(gameplaySceneData, specificFoldoutStatus);
        EditCameraOrientations(gameplaySceneData, specificFoldoutStatus);
        EditEntranceData(gameplaySceneData.EntranceData, specificFoldoutStatus);
        EditExitData(gameplaySceneData.ExitData, specificFoldoutStatus);
        EditEnemySpawnerIds(gameplaySceneData.EnemySpawnerIds, specificFoldoutStatus);
    }

    private void EditRoomTypes(
        GameplaySceneData gameplaySceneData,
        EnumDictionary<SceneSpecificFoldoutStatus, bool> specificFoldoutStatus
    )
    {
        specificFoldoutStatus[SceneSpecificFoldoutStatus.RoomTypes] = EditorGUILayout.Foldout(
            specificFoldoutStatus[SceneSpecificFoldoutStatus.RoomTypes],
            "Room Types"
        );
        if (!specificFoldoutStatus[SceneSpecificFoldoutStatus.RoomTypes]) return;
        
        EditorGUI.indentLevel++;

        List<RoomType> roomTypes = gameplaySceneData.RoomTypes;

        for (int i = 0; i < roomTypes.Count; i++)
        {
            roomTypes[i] = (RoomType) EditorGUILayout.EnumPopup("Room Type", roomTypes[i]);
        }

        EditorUtils.EditListSize("Add Room Type", "Remove Room Type", roomTypes, RoomType.Combat);

        EditorGUI.indentLevel--;
    }

    private void EditCameraOrientations(
        GameplaySceneData gameplaySceneData,
        EnumDictionary<SceneSpecificFoldoutStatus, bool> specificFoldoutStatus
    )
    {
        specificFoldoutStatus[SceneSpecificFoldoutStatus.CameraOrientations] = EditorGUILayout.Foldout(
            specificFoldoutStatus[SceneSpecificFoldoutStatus.CameraOrientations],
            "Camera Orientations"
        );
        if (!specificFoldoutStatus[SceneSpecificFoldoutStatus.CameraOrientations]) return;
        EditorGUI.indentLevel++;

        List<Pole> cameraOrientations = gameplaySceneData.CameraOrientations;

        for (int i = 0; i < cameraOrientations.Count; i++)
        {
            cameraOrientations[i] = (Pole) EditorGUILayout.EnumPopup("Orientation", cameraOrientations[i]);
        }

        EditorUtils.EditListSize("Add Orientation", "Remove Orientation", cameraOrientations, Pole.North);

        EditorGUI.indentLevel--;
    }

    private void EditEntranceData(
        List<EntranceData> entranceData,
        EnumDictionary<SceneSpecificFoldoutStatus, bool> specificFoldoutStatus
    )
    {
        specificFoldoutStatus[SceneSpecificFoldoutStatus.EntranceData] = EditorGUILayout.Foldout(
            specificFoldoutStatus[SceneSpecificFoldoutStatus.EntranceData],
            "Entrance Data"
        );
        if (!specificFoldoutStatus[SceneSpecificFoldoutStatus.EntranceData]) return;
        EditorGUI.indentLevel++;
        
        entranceData.ForEach(e =>
        {
            e.Id = EditorGUILayout.IntField("ID", e.Id);
            e.Side = (Pole) EditorGUILayout.EnumPopup("Side", e.Side);
        });

        EditorUtils.EditListSize("Add Entrance Data", "Remove Entrance Data", entranceData, () => new EntranceData());

        EditorGUI.indentLevel--;
    }

    private void EditExitData(
        List<ExitData> exitData,
        EnumDictionary<SceneSpecificFoldoutStatus, bool> specificFoldoutStatus
    )
    {
        specificFoldoutStatus[SceneSpecificFoldoutStatus.ExitData] = EditorGUILayout.Foldout(
            specificFoldoutStatus[SceneSpecificFoldoutStatus.ExitData],
            "Exit Data"
        );
        if (!specificFoldoutStatus[SceneSpecificFoldoutStatus.ExitData]) return;
        EditorGUI.indentLevel++;
        
        exitData.ForEach(e =>
        {
            e.Id = EditorGUILayout.IntField("ID", e.Id);
            e.Side = (Pole) EditorGUILayout.EnumPopup("Side", e.Side);
        });

        EditorUtils.EditListSize("Add Exit Data", "Remove Exit Data", exitData, () => new ExitData());

        EditorGUI.indentLevel--;
    }

    private void EditEnemySpawnerIds(
        List<int> enemySpawnerIds,
        EnumDictionary<SceneSpecificFoldoutStatus, bool> specificFoldoutStatus
    )
    {
        specificFoldoutStatus[SceneSpecificFoldoutStatus.EnemySpawnerIds] = EditorGUILayout.Foldout(
            specificFoldoutStatus[SceneSpecificFoldoutStatus.EnemySpawnerIds],
            "Enemy Spawner IDs"
        );
        if (!specificFoldoutStatus[SceneSpecificFoldoutStatus.EnemySpawnerIds]) return;
        EditorGUI.indentLevel++;

        for (int i = 0; i < enemySpawnerIds.Count; i++)
        {
            enemySpawnerIds[i] = EditorGUILayout.IntField("ID", enemySpawnerIds[i]);
        }

        EditorUtils.EditListSize("Add Enemy Spawner ID", "Remove Enemy Spawner ID", enemySpawnerIds, 0);

        EditorGUI.indentLevel--;
    }
}
