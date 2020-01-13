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
        if (BehaviourScanner.BehaviourDataByAction.TryGetValue(action, out List<BehaviourData> dataList))
        {
            BehaviourData selectedData = BehaviourTypeDropdown(dataList, action);
            BehaviourArgsEdit(selectedData, action);
        }
    }

    private BehaviourData BehaviourTypeDropdown(List<BehaviourData> dataList, AIAction action)
    {
        Type currentBehaviour = _ai.serializablePersonality[action].behaviour.GetType();
        int currentIndex = dataList.FindIndex(d => d.Behaviour.Equals(currentBehaviour));

        string[] displayedOptions = dataList
            .Select(d => d.DisplayValue)
            .ToArray();

        int newIndex = EditorGUILayout.Popup(action.ToString(), currentIndex, displayedOptions);

        BehaviourData selectedData = dataList[newIndex];
        if (newIndex != currentIndex)
        {
            _ai.serializablePersonality[action] = new SerializableBehaviour(
                selectedData.Behaviour,
                new float[selectedData.Args.Length]
            );
        }

        return selectedData;
    }

    private void BehaviourArgsEdit(BehaviourData selectedData, AIAction action)
    {
        EditorGUI.indentLevel++;
        float[] currentArgs = _ai.serializablePersonality[action].behaviour.Args;

        float[] newArgs = selectedData.Args
            .Zip(currentArgs, (label, currentValue) => EditorGUILayout.FloatField(label, currentValue))
            .ToArray();

        _ai.serializablePersonality[action].behaviour.Args = newArgs;
        EditorGUI.indentLevel--;
    }

    private void PlannerSelection()
    {
        EditorGUILayout.LabelField("Planners", EditorStyles.boldLabel);

        PlannerData selectedData = PlannerTypeDropdown();
        PlannerArgsEdit(selectedData);
    }

    private PlannerData PlannerTypeDropdown()
    {
        List<PlannerData> dataList = PlannerScanner.PlannerData;

        Type currentPlanner = _ai.serializablePlanner.planner.GetType();
        int currentIndex = dataList.FindIndex(d => d.Planner.Equals(currentPlanner));

        string[] displayedOptions = dataList
            .Select(d => d.DisplayValue)
            .ToArray();

        int newIndex = EditorGUILayout.Popup("Planner", currentIndex, displayedOptions);

        PlannerData selectedData = dataList[newIndex];
        if (newIndex != currentIndex)
        {
            _ai.serializablePlanner = new SerializablePlanner(
                selectedData.Planner,
                new float[selectedData.Args.Length]
            );
        }

        return selectedData;
    }

    private void PlannerArgsEdit(PlannerData selectedData)
    {
        EditorGUI.indentLevel++;
        float[] currentArgs = _ai.serializablePlanner.planner.Args;

        float[] newArgs = selectedData.Args
            .Zip(currentArgs, (label, currentValue) => EditorGUILayout.FloatField(label, currentValue))
            .ToArray();

        _ai.serializablePlanner.planner.Args = newArgs;
        EditorGUI.indentLevel--;
    }
}
