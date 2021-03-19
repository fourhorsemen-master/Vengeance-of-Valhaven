using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransitionModuleLookup))]
public class TransitionModuleLookupEditor : Editor
{
    private Dictionary<TransitionModuleData, bool> foldoutStatus;
    
    public override void OnInspectorGUI()
    {
        TransitionModuleLookup transitionModuleLookup = (TransitionModuleLookup) target;
        List<TransitionModuleData> transitionModuleDataList = transitionModuleLookup.transitionModuleDataList;

        EditorUtils.ShowScriptLink(transitionModuleLookup);
        
        EditorUtils.Header("Modules");

        if (foldoutStatus == null) InitialiseFoldoutStatus(transitionModuleDataList);

        transitionModuleDataList.ForEach(EditTransitionModalData);

        EditorUtils.EditListSize(
            "Add Module",
            "Remove Module",
            transitionModuleDataList,
            () => new TransitionModuleData(), 
            d => foldoutStatus[d] = false,
            d => foldoutStatus.Remove(d)
        );

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void InitialiseFoldoutStatus(List<TransitionModuleData> transitionModuleDataList)
    {
        foldoutStatus = transitionModuleDataList.ToDictionary(d => d, _ => false);
    }

    private void EditTransitionModalData(TransitionModuleData transitionModuleData)
    {
        EditorGUI.indentLevel++;

        transitionModuleData.Type = (TransitionModuleType) EditorGUILayout.EnumPopup("Type", transitionModuleData.Type);
        transitionModuleData.Prefab = EditorUtils.PrefabField("Prefab", transitionModuleData.Prefab);
        EditTags(transitionModuleData);
        EditorUtils.VerticalSpace();
        
        EditorGUI.indentLevel--;
    }

    private void EditTags(TransitionModuleData transitionModuleData)
    {
        foldoutStatus[transitionModuleData] = EditorGUILayout.Foldout(foldoutStatus[transitionModuleData], "Tags");

        if (!foldoutStatus[transitionModuleData]) return;

        EditorGUI.indentLevel++;

        List<ModuleTag> tags = transitionModuleData.Tags;

        for (int i = 0; i < tags.Count; i++)
        {
            tags[i] = (ModuleTag) EditorGUILayout.EnumPopup("Tag", tags[i]);
        }

        EditorUtils.EditListSize("Add Tag", "Remove Tag", tags, ModuleTag.Short);

        EditorGUI.indentLevel--;
    }
}
