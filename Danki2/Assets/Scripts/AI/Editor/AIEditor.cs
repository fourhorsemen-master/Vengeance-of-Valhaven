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
        int currentIndex = dataList.FindIndex(data => data.Behaviour.Equals(currentBehaviour));

        string[] displayedOptions = (
            from BehaviourData data
            in dataList
            select data.DisplayValue
        ).ToArray();

        int newIndex = EditorGUILayout.Popup(action.ToString(), currentIndex, displayedOptions);

        BehaviourData selectedData = dataList[newIndex];
        if (newIndex != currentIndex)
        {
            _ai.serializablePersonality[action] = new SerializableBehaviour(
                selectedData.Behaviour,
                new float[selectedData.args.Length]
            );
        }

        return selectedData;
    }

    private void BehaviourArgsEdit(BehaviourData selectedData, AIAction action)
    {
        EditorGUI.indentLevel++;
        float[] currentArgs = _ai.serializablePersonality[action].behaviour.Args;
        float[] newArgs = new float[currentArgs.Length];
        for (int i = 0; i < selectedData.args.Length; i++)
        {
            string arg = selectedData.args[i];
            newArgs[i] = EditorGUILayout.FloatField(arg, currentArgs[i]);
        }

        _ai.serializablePersonality[action].behaviour.Args = newArgs;
        EditorGUI.indentLevel--;
    }

    private void PlannerSelection()
    {
        EditorGUILayout.LabelField("Planner", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Always Advance"))
        {
            _ai.serializablePlanner = new SerializablePlanner(typeof(AlwaysAdvance), new float[0]);
        }
    }
}
