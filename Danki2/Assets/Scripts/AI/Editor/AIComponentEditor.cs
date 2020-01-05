﻿using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIComponent))]
public class AIComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AIComponent aIComponent = (AIComponent)target;

        if (GUILayout.Button("Add Follow Player AI"))
        {
            aIComponent.serializablePlanner = new SerializablePlanner(
                typeof(AlwaysAdvance), new float[0]
            );

            aIComponent.serializablePersonality[AIAction.Advance] = new SerializableBehaviour(
                typeof(FollowPlayer), new float[0]
            );
        }

        if (GUILayout.Button("Add Follow Player At Distance AI"))
        {
            aIComponent.serializablePlanner = new SerializablePlanner(
                typeof(AlwaysAdvance), new float[0]
            );

            aIComponent.serializablePersonality[AIAction.Advance] = new SerializableBehaviour(
                typeof(FollowPlayerAtDistance), new float[] { 5 }
            );
        }

        if (GUILayout.Button("Log Behaviour Attribute Values"))
        {
            BehaviourScanner.Scan();
            LogBehaviourData();
        }

        if (GUILayout.Button("Log Planner Attribute Values"))
        {
            PlannerScanner.Scan();
            LogPlannerData();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void LogBehaviourData()
    {
        foreach (AIAction action in Enum.GetValues(typeof(AIAction)))
        {
            var data = BehaviourScanner.GetDataByAction(action);
            string message = action.ToString() + " behaviours found: " + data.Count;
            if (data.Count > 0) message += ": ";
            foreach (var d in data)
            {
                message += d.DisplayValue + ", ";
            }
            message.Remove(message.Length - 2);

            Debug.Log(message);
        }
    }

    private void LogPlannerData()
    {
        string message = "planners found: " + PlannerScanner.PlannerData.Count;
        if (PlannerScanner.PlannerData.Count > 0) message += ": ";
        foreach (PlannerData data in PlannerScanner.PlannerData)
        {
            message += data.DisplayValue + ", ";
        }
        message.Remove(message.Length - 2);

        Debug.Log(message);
    }
}
