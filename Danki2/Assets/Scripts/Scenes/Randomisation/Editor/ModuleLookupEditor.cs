using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModuleLookup))]
public class ModuleLookupEditor : Editor
{
    private readonly EnumDictionary<SocketType, bool> socketFoldoutStatus = new EnumDictionary<SocketType, bool>(false);
    private EnumDictionary<SocketType, Dictionary<ModuleData, bool>> tagFoldoutStatus;
    private EnumDictionary<SocketType, Dictionary<ModuleData, bool>> rotationsFoldoutStatus;

    public override void OnInspectorGUI()
    {
        ModuleLookup moduleLookup = (ModuleLookup) target;

        EditorUtils.ShowScriptLink(moduleLookup);

        if (tagFoldoutStatus == null) InitialiseFoldoutStatuses(moduleLookup);

        EnumUtils.ForEach<SocketType>(socketType =>
        {
            socketFoldoutStatus[socketType] = EditorGUILayout.Foldout(socketFoldoutStatus[socketType], socketType.ToString());

            if (!socketFoldoutStatus[socketType]) return;

            SocketData socketData = moduleLookup.moduleDataLookup[socketType];
            socketData.SocketRotationType = (SocketRotationType) EditorGUILayout.EnumPopup(
                "Rotation Type",
                socketData.SocketRotationType
            );

            EditorUtils.VerticalSpace();

            List<ModuleData> moduleData = socketData.ModuleData;

            moduleData.ForEach(d => EditModuleData(d, socketType, socketData.SocketRotationType));

            EditorUtils.EditListSize(
                "Add Module",
                "Remove Module",
                moduleData,
                () => new ModuleData(),
                d =>
                {
                    tagFoldoutStatus[socketType][d] = false;
                    rotationsFoldoutStatus[socketType][d] = false;
                },
                d =>
                {
                    tagFoldoutStatus[socketType].Remove(d);
                    rotationsFoldoutStatus[socketType].Remove(d);
                });
        });

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void InitialiseFoldoutStatuses(ModuleLookup moduleLookup)
    {
        tagFoldoutStatus = new EnumDictionary<SocketType, Dictionary<ModuleData, bool>>(() => new Dictionary<ModuleData, bool>());
        rotationsFoldoutStatus = new EnumDictionary<SocketType, Dictionary<ModuleData, bool>>(() => new Dictionary<ModuleData, bool>());

        EnumUtils.ForEach<SocketType>(socketType =>
        {
            moduleLookup.moduleDataLookup[socketType].ModuleData.ForEach(d =>
            {
                tagFoldoutStatus[socketType][d] = false;
                rotationsFoldoutStatus[socketType][d] = false;
            });
        });
    }

    private void EditModuleData(ModuleData moduleData, SocketType socketType, SocketRotationType socketRotationType)
    {
        EditorGUI.indentLevel++;

        moduleData.Prefab = EditorUtils.PrefabField("Prefab", moduleData.Prefab);
        EditRotations(moduleData, socketType, socketRotationType);
        EditTags(moduleData.Tags, socketType, moduleData);

        EditorUtils.VerticalSpace();

        EditorGUI.indentLevel--;
    }

    private void EditRotations(ModuleData moduleData, SocketType socketType, SocketRotationType socketRotationType)
    {
        rotationsFoldoutStatus[socketType][moduleData] = EditorGUILayout.Foldout(rotationsFoldoutStatus[socketType][moduleData], "Rotations");

        if (!rotationsFoldoutStatus[socketType][moduleData]) return;

        EditorGUI.indentLevel++;
        
        switch (socketRotationType)
        {
            case SocketRotationType.Free:
                EditFreeRotation(moduleData);
                break;
            case SocketRotationType.Distinct:
                EditorDistinctRotations(moduleData);
                break;
        }

        EditorGUI.indentLevel--;
    }
    
    private void EditFreeRotation(ModuleData moduleData)
    {
        moduleData.AllowAnyFreeRotation = EditorGUILayout.Toggle("Allow Any Rotation", moduleData.AllowAnyFreeRotation);

        if (moduleData.AllowAnyFreeRotation)
        {
            moduleData.MinFreeRotation = 0;
            moduleData.MaxFreeRotation = 360;
            return;
        }

        moduleData.MinFreeRotation = EditorGUILayout.FloatField("Min Rotation", moduleData.MinFreeRotation);
        moduleData.MaxFreeRotation = EditorGUILayout.FloatField("Max Rotation", moduleData.MaxFreeRotation);
    }

    private void EditorDistinctRotations(ModuleData moduleData)
    {
        List<float> distinctRotations = moduleData.DistinctRotations;

        for (int i = 0; i < distinctRotations.Count; i++)
        {
            distinctRotations[i] = EditorGUILayout.FloatField("Rotation", distinctRotations[i]);
        }

        EditorUtils.EditListSize("Add Rotation", "Remove Rotation", distinctRotations, 0);
    }

    private void EditTags(List<ModuleTag> tags, SocketType socketType, ModuleData moduleData)
    {
        tagFoldoutStatus[socketType][moduleData] = EditorGUILayout.Foldout(tagFoldoutStatus[socketType][moduleData], "Tags");

        if (tagFoldoutStatus[socketType][moduleData])
        {
            EditorGUI.indentLevel++;
            
            for (int i = 0; i < tags.Count; i++)
            {
                tags[i] = (ModuleTag) EditorGUILayout.EnumPopup("Tag", tags[i]);
            }

            EditorUtils.EditListSize("Add Tag", "Remove Tag", tags, ModuleTag.Short);

            EditorGUI.indentLevel--;
        }
    }
}
