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

        EditColour(empowerment);

        EditorGUI.indentLevel--;
    }

    private void EditColour(Empowerment empowerment)
    {
        empowermentLookup.empowermentColourDictionary[empowerment] = EditorGUILayout.ColorField(
            "Colour",
            empowermentLookup.empowermentColourDictionary[empowerment]
        );
    }
}
