using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransitionModuleLookup))]
public class TransitionModuleLookupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TransitionModuleLookup transitionModuleLookup = (TransitionModuleLookup) target;

        EditorUtils.ShowScriptLink(transitionModuleLookup);

        EditorUtils.Header("Modules");
        EditorGUI.indentLevel++;

        EditorUtils.ResizeableList(
            transitionModuleLookup.Modules,
            module => EditorUtils.PrefabField("Prefab", module),
            defaultValue: null
        );

        EditorGUI.indentLevel--;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
