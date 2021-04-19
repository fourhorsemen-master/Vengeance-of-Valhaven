﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerationLookup))]
public class MapGenerationLookupEditor : Editor
{
    private MapGenerationLookup mapGenerationLookup;
    
    private readonly EnumDictionary<RoomType, bool> foldoutStatus = new EnumDictionary<RoomType, bool>(false);
    
    public override void OnInspectorGUI()
    {
        mapGenerationLookup = (MapGenerationLookup) target;

        EditorUtils.ShowScriptLink(mapGenerationLookup);
        
        EditLayoutData();
        EditorUtils.VerticalSpace();
        EditRoomDataLookup();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditLayoutData()
    {
        EditorUtils.Header("Layout Data");
        EditorGUI.indentLevel++;

        mapGenerationLookup.AbilityChoices = EditorGUILayout.IntField("Ability Choices", mapGenerationLookup.AbilityChoices);
        mapGenerationLookup.MinRoomDepth = EditorGUILayout.IntField("Min Room Depth", mapGenerationLookup.MinRoomDepth);
        mapGenerationLookup.MaxRoomDepth = EditorGUILayout.IntField("Max Room Depth", mapGenerationLookup.MaxRoomDepth);
        mapGenerationLookup.MinRoomExits = EditorGUILayout.IntField("Min Room Exits", mapGenerationLookup.MinRoomExits);
        mapGenerationLookup.MaxRoomExits = EditorGUILayout.IntField("Max Room Exits", mapGenerationLookup.MaxRoomExits);
        mapGenerationLookup.ChanceRoomTypeIndicatedByParent = EditorGUILayout.FloatField(
            "Chance Room Type Indicated By Parent",
            mapGenerationLookup.ChanceRoomTypeIndicatedByParent
        );
        mapGenerationLookup.ChanceRoomTypeIndicatedByGrandparent = EditorGUILayout.FloatField(
            "Chance Room Type Indicated By Grandparent",
            mapGenerationLookup.ChanceRoomTypeIndicatedByGrandparent
        );

        EditorGUI.indentLevel--;
    }

    private void EditRoomDataLookup()
    {
        EditorUtils.Header("Room Data");
        EditorGUI.indentLevel++;

        EnumUtils.ForEach<RoomType>(roomType =>
        {
            foldoutStatus[roomType] = EditorGUILayout.Foldout(foldoutStatus[roomType], roomType.ToString());
            if (!foldoutStatus[roomType]) return;

            EditorGUI.indentLevel++;
            
            EditRoomData(mapGenerationLookup.RoomDataLookup[roomType]);

            EditorGUI.indentLevel--;
            EditorUtils.VerticalSpace();
        });

        EditorGUI.indentLevel--;
    }

    private void EditRoomData(RoomData roomData)
    {
        roomData.AvailableInPool = EditorGUILayout.Toggle("Available In Pool", roomData.AvailableInPool);
        EditorGUI.BeginDisabledGroup(!roomData.AvailableInPool);

        EditorUtils.Header("Weights");
        EditorGUI.indentLevel++;

        int index = 1;
        EditorUtils.ResizeableList(
            roomData.Weights,
            weight => EditorGUILayout.IntField($"Weight {index++}", weight),
            0
        );
        
        EditorGUI.indentLevel--;
        
        EditorGUI.EndDisabledGroup();
    }
}