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

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _ai = (AI)target;

        if (!hasRunInitialScan)
        {
            Scan();
            hasRunInitialScan = true;
        }

        ScanButton();
        BehaviourSelection();
        PlannerSelection();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void ScanButton()
    {
        EditorGUILayout.LabelField("Scanning", EditorStyles.boldLabel);

        if (GUILayout.Button("Scan"))
        {
            Scan();
        }
    }

    private void Scan()
    {
        BehaviourScanner.Scan();
        PlannerScanner.Scan();
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
        int currentIndex = dataList.FindIndex(data => data.Behaviour.Equals(_ai.serializablePersonality[action].behaviour.GetType()));

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
                Enumerable.Repeat(0f, selectedData.args.Length).ToArray()
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
