using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModuleLookup))]
public class ModuleLookupEditor : Editor
{
    private readonly EnumDictionary<SocketType, bool> foldoutStatus = new EnumDictionary<SocketType, bool>(false);

    public override void OnInspectorGUI()
    {
        ModuleLookup moduleLookup = (ModuleLookup) target;
        
        EditorUtils.ShowScriptLink(moduleLookup);

        EnumUtils.ForEach<SocketType>(socketType =>
        {
            foldoutStatus[socketType] = EditorGUILayout.Foldout(foldoutStatus[socketType], socketType.ToString());

            if (!foldoutStatus[socketType]) return;

            EditorGUI.indentLevel++;

            List<ModuleData> moduleDataList = moduleLookup.moduleDataLookup[socketType].List;
            
            moduleDataList.ForEach(EditModuleData);

            EditorUtils.EditListSize("Add Module", "Remove Module", moduleDataList, () => new ModuleData());

            EditorGUI.indentLevel--;
        });

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditModuleData(ModuleData moduleData)
    {
        EditorGUI.indentLevel++;

        moduleData.Prefab = EditorUtils.PrefabField("Prefab", moduleData.Prefab);
        EditTags(moduleData.Tags);

        EditorGUI.indentLevel--;
    }

    private void EditTags(List<ModuleTag> tags)
    {
        EditorUtils.Header("Tags");

        EditorGUI.indentLevel++;
        
        for (int i = 0; i < tags.Count; i++)
        {
            tags[i] = (ModuleTag) EditorGUILayout.EnumPopup("Tag", tags[i]);
        }

        EditorUtils.EditListSize("Add Tag", "Remove Tag", tags, ModuleTag.Short);

        EditorUtils.VerticalSpace();

        EditorGUI.indentLevel--;
    }
}
