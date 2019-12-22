using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIComponent))]
public class AIComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Log Attribute Values"))
        {
            BehaviourScanner.Scan();
            LogBehaviourData();
            InstantiateBehaviours();
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

    private void InstantiateBehaviours()
    {
        foreach (AIAction action in Enum.GetValues(typeof(AIAction)))
        {
            float[] test = new float[] { 5 };

            var data = BehaviourScanner.GetDataByAction(action)[0];
            Activator.CreateInstance(data.Behaviour, test);
        }
    }
}
