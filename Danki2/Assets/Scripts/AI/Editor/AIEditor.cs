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

        if (GUILayout.Button("Rescan AI"))
        {
            Scan();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void ScanIfNotScanned()
    {
        if (!hasRunInitialScan)
        {
            Scan();
            hasRunInitialScan = true;
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
        if (BehaviourScanner.BehaviourDataByAction.TryGetValue(action, out List<AttributeData<BehaviourAttribute>> dataList))
        {
            AttributeData<BehaviourAttribute> selectedData = DropdownEdit(
                dataList,
                _ai.serializablePersonality[action].aiElement,
                action.ToString(),
                newData => {
                    _ai.serializablePersonality[action] = new SerializableBehaviour(
                        newData.Type,
                        new float[newData.Attribute.ArgLabels.Length]
                    );
                }
            );
            ArgsEdit(_ai.serializablePersonality[action].aiElement, selectedData.Attribute);
        }
    }

    private void PlannerSelection()
    {
        EditorGUILayout.LabelField("Planners", EditorStyles.boldLabel);

        AttributeData<PlannerAttribute> selectedData = DropdownEdit(
            PlannerScanner.PlannerData,
            _ai.serializablePlanner.aiElement,
            "Planner",
            newData => {
                _ai.serializablePlanner = new SerializablePlanner(
                    newData.Type,
                    new float[newData.Attribute.ArgLabels.Length]
                );
            }
        );
        ArgsEdit(_ai.serializablePlanner.aiElement, selectedData.Attribute);
    }

    private AttributeData<T> DropdownEdit<T>(
        List<AttributeData<T>> dataList,
        AIElement aiElement,
        string label,
        Action<AttributeData<T>> dataChangeCallback
    ) where T : AIAttribute
    {
        int currentIndex = dataList.FindIndex(d => d.Type.Equals(aiElement.GetType()));

        string[] displayedOptions = dataList
            .Select(d => d.Attribute.DisplayValue)
            .ToArray();

        int newIndex = EditorGUILayout.Popup(label, currentIndex, displayedOptions);

        if (newIndex != currentIndex)
        {
            dataChangeCallback.Invoke(dataList[newIndex]);
        }

        return dataList[newIndex];
    }

    private void ArgsEdit(AIElement toUpdate, AIAttribute toGetLabelsFrom)
    {
        EditorGUI.indentLevel++;
        float[] currentArgs = toUpdate.Args;

        float[] newArgs = toGetLabelsFrom.ArgLabels
            .Zip(currentArgs, (label, currentValue) => EditorGUILayout.FloatField(label, currentValue))
            .ToArray();

        if (!Enumerable.SequenceEqual(currentArgs, newArgs))
        {
            toUpdate.Args = newArgs;
        }
        EditorGUI.indentLevel--;
    }
}
