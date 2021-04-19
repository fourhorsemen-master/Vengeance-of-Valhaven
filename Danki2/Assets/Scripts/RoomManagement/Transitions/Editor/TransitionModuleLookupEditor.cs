using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransitionModuleLookup))]
public class TransitionModuleLookupEditor : Editor
{
    private TransitionModuleLookup transitionModuleLookup;
    
    private readonly EnumDictionary<RoomType, bool> foldoutStatus = new EnumDictionary<RoomType, bool>(false);
    
    public override void OnInspectorGUI()
    {
        transitionModuleLookup = (TransitionModuleLookup) target;

        EditorUtils.ShowScriptLink(transitionModuleLookup);
        
        EditGenericModules();
        EditTransitionModuleDictionary();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditGenericModules()
    {
        EditorUtils.Header("Generic Modules");
        EditorGUI.indentLevel++;
        
        EditorUtils.ResizeableList(
            transitionModuleLookup.GenericModules,
            transitionModule => EditorUtils.PrefabField("Prefab", transitionModule),
            defaultValue: null
        );

        EditorGUI.indentLevel--;
    }

    private void EditTransitionModuleDictionary()
    {
        EnumUtils.ForEach<RoomType>(roomType =>
        {
            foldoutStatus[roomType] = EditorGUILayout.Foldout(foldoutStatus[roomType], roomType.ToString());
            if (!foldoutStatus[roomType]) return;

            EditorGUI.indentLevel++;
            EditTransitionModuleData(transitionModuleLookup.TransitionModuleDictionary[roomType]);
            EditorGUI.indentLevel--;
        });
    }

    private void EditTransitionModuleData(TransitionModuleData transitionModuleData)
    {
        EditorUtils.Header("Transition Prefabs");
        EditorGUI.indentLevel++;
        
        EditorUtils.ResizeableList(
            transitionModuleData.TransitionPrefabs,
            transitionModule => EditorUtils.PrefabField("Prefab", transitionModule),
            defaultValue: null
        );
        
        EditorGUI.indentLevel--;
        
        EditorUtils.Header("Indicator Prefabs");
        EditorGUI.indentLevel++;
        
        EditorUtils.ResizeableList(
            transitionModuleData.IndicatorPrefabs,
            gameObject => EditorUtils.PrefabField("Prefab", gameObject),
            defaultValue: null
        );
        
        EditorGUI.indentLevel--;
    }
}
