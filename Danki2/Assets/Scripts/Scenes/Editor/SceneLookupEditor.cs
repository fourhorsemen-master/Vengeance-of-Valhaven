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

        EditorUtils.ResizeableList(
            gameplaySceneData.RoomTypes,
            roomType => (RoomType) EditorGUILayout.EnumPopup("Room Type", roomType),
            RoomType.Combat
        );

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

        EditorUtils.ResizeableList(
            gameplaySceneData.CameraOrientations,
            cameraOrientation => (Pole) EditorGUILayout.EnumPopup("Orientation", cameraOrientation),
            Pole.North
        );

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
        
        EditorUtils.ResizeableList(
            entranceData,
            d =>
            {
                d.Id = EditorGUILayout.IntField("ID", d.Id);
                d.Side = (Pole) EditorGUILayout.EnumPopup("Side", d.Side);
            },
            () => new EntranceData()
        );

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
        
        EditorUtils.ResizeableList(
            exitData,
            d =>
            {
                d.Id = EditorGUILayout.IntField("ID", d.Id);
                d.Side = (Pole) EditorGUILayout.EnumPopup("Side", d.Side);
            },
            () => new ExitData()
        );

        EditorGUI.indentLevel--;
    }
}
