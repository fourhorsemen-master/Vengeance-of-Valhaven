using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AI))]
public class AIEditor : Editor
{
    private bool hasRunInitialScan = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AI ai = (AI)target;

        if (!hasRunInitialScan)
        {
            Scan();
            hasRunInitialScan = true;
        }

        ScanButton();
        BehaviourSelection(ai);
        PlannerSelection(ai);

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

    private void BehaviourSelection(AI ai)
    {
        EditorGUILayout.LabelField("Behaviours", EditorStyles.boldLabel);

        foreach (AIAction action in Enum.GetValues(typeof(AIAction)))
        {
            EditBehaviour(ai, action);
        }
    }

    private void EditBehaviour(AI ai, AIAction action)
    {
        if (BehaviourScanner.BehaviourDataByAction.TryGetValue(action, out List<BehaviourData> dataList))
        {
            int currentIndex = dataList.FindIndex(data => data.Behaviour.Equals(ai.serializablePersonality[action].behaviour.GetType()));

            string[] displayedOptions = (
                from BehaviourData data
                in dataList
                select data.DisplayValue
            ).ToArray();

            int newIndex = EditorGUILayout.Popup(action.ToString(), currentIndex, displayedOptions);

            BehaviourData selectedData = dataList[newIndex];
            if (newIndex != currentIndex)
            {
                ai.serializablePersonality[action] = new SerializableBehaviour(
                    selectedData.Behaviour,
                    Enumerable.Repeat(0f, selectedData.args.Length).ToArray()
                );
            }

            EditorGUI.indentLevel++;
            float[] currentArgs = ai.serializablePersonality[action].behaviour.Args;
            float[] newArgs = new float[currentArgs.Length];
            for (int i = 0; i < selectedData.args.Length; i++)
            {
                string arg = selectedData.args[i];
                newArgs[i] = EditorGUILayout.FloatField(arg, currentArgs[i]);
            }

            ai.serializablePersonality[action].behaviour.Args = newArgs;
            EditorGUI.indentLevel--;
        }
    }

    private void PlannerSelection(AI ai)
    {
        EditorGUILayout.LabelField("Planner", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Always Advance"))
        {
            ai.serializablePlanner = new SerializablePlanner(typeof(AlwaysAdvance), new float[0]);
        }
    }
}
