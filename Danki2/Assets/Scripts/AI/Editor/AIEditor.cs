using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AI))]
public class AIEditor : Editor
{
    private bool hasRunInitialScan = false;

    private AI _ai = null;

    private void OnValidate()
    {
        ScanIfNotScanned();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _ai = (AI)target;

        ScanIfNotScanned();

        BehaviourSelection();
        PlannerSelection();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void ScanIfNotScanned()
    {
        if (!hasRunInitialScan)
        {
            BehaviourScanner.Scan();
            PlannerScanner.Scan();
            hasRunInitialScan = true;
        }
    }

    private void BehaviourSelection()
    {
        EditorGUILayout.LabelField("Behaviours", EditorStyles.boldLabel);

        foreach (AIAction action in Enum.GetValues(typeof(AIAction)))
        {
            EditBehaviour(action);
        }
    }

    private void EditBehaviour(AIAction action)
    {
        if (BehaviourScanner.BehaviourDataByAction.TryGetValue(action, out List<AttributeData<BehaviourAttribute>> dataList))
        {
            AttributeData<BehaviourAttribute> selectedData = BehaviourTypeDropdown(dataList, action);
            BehaviourArgsEdit(selectedData, action);
        }
    }

    private AttributeData<BehaviourAttribute> BehaviourTypeDropdown(List<AttributeData<BehaviourAttribute>> dataList, AIAction action)
    {
        Type currentBehaviour = _ai.serializablePersonality[action].behaviour.GetType();
        int currentIndex = dataList.FindIndex(d => d.Type.Equals(currentBehaviour));

        string[] displayedOptions = dataList
            .Select(d => d.Attribute.DisplayValue)
            .ToArray();

        int newIndex = EditorGUILayout.Popup(action.ToString(), currentIndex, displayedOptions);

        AttributeData<BehaviourAttribute> selectedData = dataList[newIndex];
        if (newIndex != currentIndex)
        {
            _ai.serializablePersonality[action] = new SerializableBehaviour(
                selectedData.Type,
                new float[selectedData.Attribute.Args.Length]
            );
        }

        return selectedData;
    }

    private void BehaviourArgsEdit(AttributeData<BehaviourAttribute> selectedData, AIAction action)
    {
        EditorGUI.indentLevel++;
        float[] currentArgs = _ai.serializablePersonality[action].behaviour.Args;

        float[] newArgs = selectedData.Attribute.Args
            .Zip(currentArgs, (label, currentValue) => EditorGUILayout.FloatField(label, currentValue))
            .ToArray();

        _ai.serializablePersonality[action].behaviour.Args = newArgs;
        EditorGUI.indentLevel--;
    }

    private void PlannerSelection()
    {
        EditorGUILayout.LabelField("Planners", EditorStyles.boldLabel);

        AttributeData<PlannerAttribute> selectedData = PlannerTypeDropdown();
        PlannerArgsEdit(selectedData);
    }

    private AttributeData<PlannerAttribute> PlannerTypeDropdown()
    {
        List<AttributeData<PlannerAttribute>> dataList = PlannerScanner.PlannerData;

        Type currentPlanner = _ai.serializablePlanner.planner.GetType();
        int currentIndex = dataList.FindIndex(d => d.Type.Equals(currentPlanner));

        string[] displayedOptions = dataList
            .Select(d => d.Attribute.DisplayValue)
            .ToArray();

        int newIndex = EditorGUILayout.Popup("Planner", currentIndex, displayedOptions);

        AttributeData<PlannerAttribute> selectedData = dataList[newIndex];
        if (newIndex != currentIndex)
        {
            _ai.serializablePlanner = new SerializablePlanner(
                selectedData.Type,
                new float[selectedData.Attribute.Args.Length]
            );
        }

        return selectedData;
    }

    private void PlannerArgsEdit(AttributeData<PlannerAttribute> selectedData)
    {
        EditorGUI.indentLevel++;
        float[] currentArgs = _ai.serializablePlanner.planner.Args;

        float[] newArgs = selectedData.Attribute.Args
            .Zip(currentArgs, (label, currentValue) => EditorGUILayout.FloatField(label, currentValue))
            .ToArray();

        _ai.serializablePlanner.planner.Args = newArgs;
        EditorGUI.indentLevel--;
    }
}
