using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModuleLookup))]
public class ModuleLookupEditor : Editor
{
    private readonly EnumDictionary<SocketType, bool> socketFoldoutStatus = new EnumDictionary<SocketType, bool>(false);
    private EnumDictionary<SocketType, Dictionary<ModuleData, bool>> zoneFoldoutStatus;
    private EnumDictionary<SocketType, Dictionary<ModuleData, bool>> rotationsFoldoutStatus;
    private EnumDictionary<SocketType, Dictionary<ModuleData, bool>> tagFoldoutStatus;

    public override void OnInspectorGUI()
    {
        ModuleLookup moduleLookup = (ModuleLookup) target;

        EditorUtils.ShowScriptLink(moduleLookup);

        if (zoneFoldoutStatus == null) InitialiseFoldoutStatuses(moduleLookup);

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

            EditorUtils.ResizeableList(
                socketData.ModuleData,
                moduleData => EditModuleData(moduleData, socketType, socketData.SocketRotationType),
                () => new ModuleData(),
                moduleData =>
                {
                    zoneFoldoutStatus[socketType][moduleData] = false;
                    rotationsFoldoutStatus[socketType][moduleData] = false;
                    tagFoldoutStatus[socketType][moduleData] = false;
                },
                moduleData =>
                {
                    zoneFoldoutStatus[socketType].Remove(moduleData);
                    rotationsFoldoutStatus[socketType].Remove(moduleData);
                    tagFoldoutStatus[socketType].Remove(moduleData);
                }
            );
        });

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void InitialiseFoldoutStatuses(ModuleLookup moduleLookup)
    {
        zoneFoldoutStatus = new EnumDictionary<SocketType, Dictionary<ModuleData, bool>>(() => new Dictionary<ModuleData, bool>());
        rotationsFoldoutStatus = new EnumDictionary<SocketType, Dictionary<ModuleData, bool>>(() => new Dictionary<ModuleData, bool>());
        tagFoldoutStatus = new EnumDictionary<SocketType, Dictionary<ModuleData, bool>>(() => new Dictionary<ModuleData, bool>());

        EnumUtils.ForEach<SocketType>(socketType =>
        {
            moduleLookup.moduleDataLookup[socketType].ModuleData.ForEach(d =>
            {
                zoneFoldoutStatus[socketType][d] = false;
                rotationsFoldoutStatus[socketType][d] = false;
                tagFoldoutStatus[socketType][d] = false;
            });
        });
    }

    private void EditModuleData(ModuleData moduleData, SocketType socketType, SocketRotationType socketRotationType)
    {
        EditorGUI.indentLevel++;

        moduleData.Prefab = EditorUtils.PrefabField("Prefab", moduleData.Prefab);
        EditZones(moduleData.Zones, socketType, moduleData);
        EditRotations(moduleData, socketType, socketRotationType);
        EditTags(moduleData.Tags, socketType, moduleData);

        EditorUtils.VerticalSpace();

        EditorGUI.indentLevel--;
    }

    private void EditZones(List<Zone> zones, SocketType socketType, ModuleData moduleData)
    {
        zoneFoldoutStatus[socketType][moduleData] = EditorGUILayout.Foldout(zoneFoldoutStatus[socketType][moduleData], "Zones");

        if (zoneFoldoutStatus[socketType][moduleData])
        {
            EditorGUI.indentLevel++;
            
            EditorUtils.ResizeableList(
                zones,
                zone => (Zone) EditorGUILayout.EnumPopup("Zone", zone),
                Zone.Zone1
            );

            EditorGUI.indentLevel--;
        }
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
        EditorUtils.ResizeableList(
            moduleData.DistinctRotations,
            rotation => EditorGUILayout.FloatField("Rotation", rotation),
            0
        );
    }

    private void EditTags(List<ModuleTag> tags, SocketType socketType, ModuleData moduleData)
    {
        tagFoldoutStatus[socketType][moduleData] = EditorGUILayout.Foldout(tagFoldoutStatus[socketType][moduleData], "Tags");

        if (tagFoldoutStatus[socketType][moduleData])
        {
            EditorGUI.indentLevel++;
            
            EditorUtils.ResizeableList(
                tags,
                tag => (ModuleTag) EditorGUILayout.EnumPopup("Tag", tag),
                ModuleTag.Short
            );

            EditorGUI.indentLevel--;
        }
    }
}
