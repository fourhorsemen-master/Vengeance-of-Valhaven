using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EmpowermentLookup))]
public class EmpowermentLookupEditor : Editor
{
    private readonly EnumDictionary<Empowerment, bool> foldoutStatus = new EnumDictionary<Empowerment, bool>(false);

    private EmpowermentLookup empowermentLookup;
    
    public override void OnInspectorGUI()
    {
        empowermentLookup = (EmpowermentLookup) target;
        
        EditorUtils.ShowScriptLink(empowermentLookup);
        
        ExpandAndCollapseAll();
        
        EnumUtils.ForEach<Empowerment>(EditEmpowermentData);
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
    
    private void ExpandAndCollapseAll()
    {
        GUILayout.BeginHorizontal();
        if (foldoutStatus.Values.All(value => value))
        {
            if (GUILayout.Button("Collapse All")) SetAllFoldoutStatuses(false);
        }
        else
        {
            if (GUILayout.Button("Expand All")) SetAllFoldoutStatuses(true);
        }
        GUILayout.EndHorizontal();
    }

    private void SetAllFoldoutStatuses(bool value)
    {
        EnumUtils.ForEach<Empowerment>(empowerment => foldoutStatus[empowerment] = value);
    }

    private void EditEmpowermentData(Empowerment empowerment)
    {
        foldoutStatus[empowerment] = EditorGUILayout.Foldout(foldoutStatus[empowerment], empowerment.ToString());
        if (!foldoutStatus[empowerment]) return;

        EditorGUI.indentLevel++;

        EditDisplayName(empowerment);
        EditTooltip(empowerment);
        EditSupplementaryTooltips(empowerment);
        EditColour(empowerment);
        EditDecalMaterial(empowerment);

        EditorGUI.indentLevel--;
    }

    private void EditDisplayName(Empowerment empowerment)
    {
        empowermentLookup.EmpowermentDisplayNameDictionary[empowerment] = EditorGUILayout.TextField(
            "Display Name",
            empowermentLookup.EmpowermentDisplayNameDictionary[empowerment]
        );
    }

    private void EditTooltip(Empowerment empowerment)
    {
        empowermentLookup.EmpowermentTooltipDictionary[empowerment] = EditorGUILayout.TextField(
            "Tooltip",
            empowermentLookup.EmpowermentTooltipDictionary[empowerment]
        );
    }

    private void EditSupplementaryTooltips(Empowerment empowerment)
    {
        EditorUtils.Header("SupplementaryTooltips");

        EditorGUI.indentLevel++;
        
        EditorUtils.Header("Active Effects");
        EditorGUI.indentLevel++;
        EditorUtils.ResizeableList(
            empowermentLookup.EmpowermentActiveEffectsDictionary[empowerment].ActiveEffects,
            activeEffect => (ActiveEffect) EditorGUILayout.EnumPopup("Active Effect", activeEffect),
            defaultValue: default
        );
        EditorGUI.indentLevel--;
        
        EditorUtils.Header("Passive Effects");
        EditorGUI.indentLevel++;
        EditorUtils.ResizeableList(
            empowermentLookup.EmpowermentPassiveEffectsDictionary[empowerment].PassiveEffects,
            passiveEffect => (PassiveEffect) EditorGUILayout.EnumPopup("Passive Effect", passiveEffect),
            defaultValue: default
        );
        EditorGUI.indentLevel--;
        
        EditorUtils.Header("Stacking Effects");
        EditorGUI.indentLevel++;
        EditorUtils.ResizeableList(
            empowermentLookup.EmpowermentStackingEffectsDictionary[empowerment].StackingEffects,
            stackingEffect => (StackingEffect) EditorGUILayout.EnumPopup("Stacking Effect", stackingEffect),
            defaultValue: default
        );
        EditorGUI.indentLevel--;
        
        EditorGUI.indentLevel--;
    }

    private void EditColour(Empowerment empowerment)
    {
        empowermentLookup.EmpowermentColourDictionary[empowerment] = EditorGUILayout.ColorField(
            "Colour",
            empowermentLookup.EmpowermentColourDictionary[empowerment]
        );
    }

    private void EditDecalMaterial(Empowerment empowerment)
    {
        empowermentLookup.EmpowermentDecalMaterialDictionary[empowerment] = (Material)EditorGUILayout.ObjectField(
            "Material",
            empowermentLookup.EmpowermentDecalMaterialDictionary[empowerment],
            typeof(Material),
            false
        );
    }
}
