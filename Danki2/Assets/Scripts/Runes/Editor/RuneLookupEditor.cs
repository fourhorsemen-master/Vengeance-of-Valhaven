using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RuneLookup))]
public class RuneLookupEditor : Editor
{
    private RuneLookup runeLookup;
    
    private readonly EnumDictionary<Rune, bool> foldoutStatus = new EnumDictionary<Rune, bool>(false);
    
    public override void OnInspectorGUI()
    {
        runeLookup = (RuneLookup) target;
        
        EditorUtils.ShowScriptLink(runeLookup);
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Expand All")) SetAllFoldoutStatuses(true);
        if (GUILayout.Button("Collapse All")) SetAllFoldoutStatuses(false);
        GUILayout.EndHorizontal();
        
        EnumUtils.ForEach<Rune>(EditRuneData);
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void SetAllFoldoutStatuses(bool value)
    {
        EnumUtils.ForEach<Rune>(rune => foldoutStatus[rune] = value);
    }
    
    private void EditRuneData(Rune rune)
    {
        foldoutStatus[rune] = EditorGUILayout.Foldout(foldoutStatus[rune], rune.ToString());
        if (!foldoutStatus[rune]) return;
        
        EditorGUI.indentLevel++;

        RuneData runeData = runeLookup.runeDataLookup[rune];
        runeData.DisplayName = EditorGUILayout.TextField("Display Name", runeData.DisplayName);
        runeData.Tooltip = EditorUtils.MultilineTextField("Tooltip", runeData.Tooltip, 3);
        EditSupplementaryTooltips(rune);
        runeData.Sprite = (Sprite) EditorGUILayout.ObjectField("Sprite", runeData.Sprite, typeof(Sprite), false);
        
        EditorGUI.indentLevel--;
    }

    private void EditSupplementaryTooltips(Rune rune)
    {
        EditorUtils.Header("SupplementaryTooltips");

        EditorGUI.indentLevel++;
        
        EditorUtils.Header("Active Effects");
        EditorGUI.indentLevel++;
        EditorUtils.ResizeableList(
            runeLookup.GetActiveEffects(rune),
            activeEffect => (ActiveEffect) EditorGUILayout.EnumPopup("Active Effect", activeEffect),
            defaultValue: default
        );
        EditorGUI.indentLevel--;
        
        EditorUtils.Header("Passive Effects");
        EditorGUI.indentLevel++;
        EditorUtils.ResizeableList(
            runeLookup.GetPassiveEffects(rune),
            passiveEffect => (PassiveEffect) EditorGUILayout.EnumPopup("Passive Effect", passiveEffect),
            defaultValue: default
        );
        EditorGUI.indentLevel--;
        
        EditorUtils.Header("Stacking Effects");
        EditorGUI.indentLevel++;
        EditorUtils.ResizeableList(
            runeLookup.GetStackingEffects(rune),
            stackingEffect => (StackingEffect) EditorGUILayout.EnumPopup("Stacking Effect", stackingEffect),
            defaultValue: default
        );
        EditorGUI.indentLevel--;
        
        EditorGUI.indentLevel--;
    }
}
