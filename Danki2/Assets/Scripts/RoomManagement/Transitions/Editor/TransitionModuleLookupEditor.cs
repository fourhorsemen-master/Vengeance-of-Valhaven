using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransitionModuleLookup))]
public class TransitionModuleLookupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TransitionModuleLookup transitionModuleLookup = (TransitionModuleLookup) target;

        EditorUtils.ShowScriptLink(transitionModuleLookup);

        List<GameObject> modules = transitionModuleLookup.Modules;

        EditorUtils.Header("Modules");
        EditorGUI.indentLevel++;

        for (int i = 0; i < modules.Count; i++)
        {
            modules[i] = EditorUtils.PrefabField("Prefab", modules[i]);
        }

        EditorUtils.EditListSize("Add Module", "Remove Module", modules, () => null);

        EditorGUI.indentLevel--;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
