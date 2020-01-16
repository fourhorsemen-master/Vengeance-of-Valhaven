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

        if (GUILayout.Button("Scan"))
        {
            Scan();
        }

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
            //_ai.serializablePersonality[action] = new SerializableBehaviour(
            //    selectedData.Type,
            //    new float[selectedData.Attribute.ArgLabels.Length]
            //);
            ArgsEdit(_ai.serializablePersonality[action].aiElement, selectedData.Attribute);



            //AttributeData<PlannerAttribute> selectedData = DropdownEdit(
            //    PlannerScanner.PlannerData,
            //    _ai.serializablePlanner.aiElement.GetType(),
            //    "Planner"
            //);
            //_ai.serializablePlanner = new SerializablePlanner(
            //    selectedData.Type,
            //    new float[selectedData.Attribute.ArgLabels.Length]
            //);
            //ArgsEdit(_ai.serializablePlanner.aiElement, selectedData.Attribute);
        }
    }

    //private AttributeData<BehaviourAttribute> BehaviourTypeDropdown(List<AttributeData<BehaviourAttribute>> dataList, AIAction action)
    //{
    //    Type currentBehaviour = _ai.serializablePersonality[action].behaviour.GetType();
    //    int currentIndex = dataList.FindIndex(d => d.Type.Equals(currentBehaviour));

    //    string[] displayedOptions = dataList
    //        .Select(d => d.Attribute.DisplayValue)
    //        .ToArray();

    //    int newIndex = EditorGUILayout.Popup(action.ToString(), currentIndex, displayedOptions);

    //    AttributeData<BehaviourAttribute> selectedData = dataList[newIndex];
    //    if (newIndex != currentIndex)
    //    {
    //        _ai.serializablePersonality[action] = new SerializableBehaviour(
    //            selectedData.Type,
    //            new float[selectedData.Attribute.ArgLabels.Length]
    //        );
    //    }

    //    return selectedData;
    //}

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
        //_ai.serializablePlanner = new SerializablePlanner(
        //    selectedData.Type,
        //    new float[selectedData.Attribute.ArgLabels.Length]
        //);
        ArgsEdit(_ai.serializablePlanner.aiElement, selectedData.Attribute);
    }

    #region
    //private AttributeData<PlannerAttribute> PlannerTypeDropdown()
    //{
    //    List<AttributeData<PlannerAttribute>> dataList = PlannerScanner.PlannerData;

    //    Type currentPlanner = _ai.serializablePlanner.planner.GetType();
    //    int currentIndex = dataList.FindIndex(d => d.Type.Equals(currentPlanner));

    //    string[] displayedOptions = dataList
    //        .Select(d => d.Attribute.DisplayValue)
    //        .ToArray();

    //    int newIndex = EditorGUILayout.Popup("Planner", currentIndex, displayedOptions);

    //    AttributeData<PlannerAttribute> selectedData = dataList[newIndex];
    //    if (newIndex != currentIndex)
    //    {
    //        _ai.serializablePlanner = new SerializablePlanner(
    //            selectedData.Type,
    //            new float[selectedData.Attribute.ArgLabels.Length]
    //        );
    //    }

    //    return selectedData;
    //}
    #endregion

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
            //aiElement = (AIElement)Activator.CreateInstance(
            //    selected
            //    aiElement.GetType(),
            //    new object[] { aiElement.GetType(), new float[aiElement.Args.Length] }
            //);
        }

        return dataList[newIndex];
        //if (newIndex != currentIndex)
        //{
        //    _ai.serializablePlanner = new SerializablePlanner(
        //        selectedData.Type,
        //        new float[selectedData.Attribute.ArgLabels.Length]
        //    );
        //}

        //return selectedData;
    }

    private void ArgsEdit(AIElement toUpdate, AIAttribute toGetLabelsFrom)
    {
        EditorGUI.indentLevel++;
        float[] currentArgs = toUpdate.Args;

        float[] newArgs = toGetLabelsFrom.ArgLabels
            .Zip(currentArgs, (label, currentValue) => EditorGUILayout.FloatField(label, currentValue))
            .ToArray();

        toUpdate.Args = newArgs;
        EditorGUI.indentLevel--;
    }
}
