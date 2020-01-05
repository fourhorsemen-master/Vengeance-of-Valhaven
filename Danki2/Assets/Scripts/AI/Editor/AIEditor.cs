using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AI))]
public class AIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AI ai = (AI)target;

        ScanButtons();
        BehaviourSelection(ai);
        PlannerSelection(ai);

        //if (GUILayout.Button("Add Follow Player AI"))
        //{
        //    ai.serializablePlanner = new SerializablePlanner(
        //        typeof(AlwaysAdvance), new float[0]
        //    );

        //    ai.serializablePersonality[AIAction.Advance] = new SerializableBehaviour(
        //        typeof(FollowPlayer), new float[0]
        //    );
        //}

        //if (GUILayout.Button("Add Follow Player At Distance AI"))
        //{
        //    ai.serializablePlanner = new SerializablePlanner(
        //        typeof(AlwaysAdvance), new float[0]
        //    );

        //    ai.serializablePersonality[AIAction.Advance] = new SerializableBehaviour(
        //        typeof(FollowPlayerAtDistance), new float[] { 5 }
        //    );
        //}

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void ScanButtons()
    {
        EditorGUILayout.LabelField("Scanning", EditorStyles.boldLabel);

        if (GUILayout.Button("Scan For Behaviours"))
        {
            BehaviourScanner.Scan();
        }

        if (GUILayout.Button("Scan For Planners"))
        {
            PlannerScanner.Scan();
        }
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
            int selectedIndex = dataList.FindIndex(data => data.Behaviour.Equals(ai.serializablePersonality[action]._serializableBehaviourType.Type));

            string[] displayedOptions = (
                from BehaviourData data
                in dataList
                select data.DisplayValue
            ).ToArray();

            EditorGUILayout.Popup(action.ToString(), selectedIndex, displayedOptions);
        }
    }

    private void PlannerSelection(AI ai)
    {
        EditorGUILayout.LabelField("Planner", EditorStyles.boldLabel);
    }
}
