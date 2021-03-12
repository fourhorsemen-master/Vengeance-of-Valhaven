using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModuleLookup))]
public class ModuleLookupEditor : Editor
{
    private readonly EnumDictionary<SocketType, bool> socketFoldoutStatus = new EnumDictionary<SocketType, bool>(false);
    private EnumDictionary<SocketType, Dictionary<ModuleData, bool>> tagFoldoutStatus;

    public override void OnInspectorGUI()
    {
        ModuleLookup moduleLookup = (ModuleLookup) target;

        EditorUtils.ShowScriptLink(moduleLookup);

        if (tagFoldoutStatus == null) InitialiseTagFoldoutStatus(moduleLookup);

        EnumUtils.ForEach<SocketType>(socketType =>
        {
            socketFoldoutStatus[socketType] = EditorGUILayout.Foldout(socketFoldoutStatus[socketType], socketType.ToString());

            if (!socketFoldoutStatus[socketType]) return;

            List<ModuleData> moduleDataList = moduleLookup.moduleDataLookup[socketType].List;
            
            moduleDataList.ForEach(d => EditModuleData(d, socketType));

            EditorUtils.EditListSize(
                "Add Module",
                "Remove Module",
                moduleDataList,
                () => new ModuleData(),
                d => tagFoldoutStatus[socketType][d] = false,
                d => tagFoldoutStatus[socketType].Remove(d)
            );
        });

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void InitialiseTagFoldoutStatus(ModuleLookup moduleLookup)
    {
        tagFoldoutStatus = new EnumDictionary<SocketType, Dictionary<ModuleData, bool>>(() => new Dictionary<ModuleData, bool>());

        EnumUtils.ForEach<SocketType>(socketType =>
        {
            moduleLookup.moduleDataLookup[socketType].List.ForEach(d => tagFoldoutStatus[socketType][d] = false);
        });
    }

    private void EditModuleData(ModuleData moduleData, SocketType socketType)
    {
        EditorGUI.indentLevel++;

        moduleData.Prefab = EditorUtils.PrefabField("Prefab", moduleData.Prefab);
        EditTags(moduleData.Tags, socketType, moduleData);

        EditorGUI.indentLevel--;
    }

    private void EditTags(List<ModuleTag> tags, SocketType socketType, ModuleData moduleData)
    {
        tagFoldoutStatus[socketType][moduleData] = EditorGUILayout.Foldout(tagFoldoutStatus[socketType][moduleData], "Tags");

        if (tagFoldoutStatus[socketType][moduleData])
        {
            for (int i = 0; i < tags.Count; i++)
            {
                tags[i] = (ModuleTag) EditorGUILayout.EnumPopup("Tag", tags[i]);
            }

            EditorUtils.EditListSize("Add Tag", "Remove Tag", tags, ModuleTag.Short);
        }

        EditorUtils.VerticalSpace();
    }
}
