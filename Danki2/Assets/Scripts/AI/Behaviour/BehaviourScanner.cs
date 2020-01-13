using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BehaviourScanner
{
    public static List<BehaviourData> BehaviourData { get; private set; } = new List<BehaviourData>();
    public static Dictionary<AIAction, List<BehaviourData>> BehaviourDataByAction { get; private set; } = new Dictionary<AIAction, List<BehaviourData>>();

    public static void Scan()
    {
        BehaviourData.Clear();
        List<AttributeData<BehaviourAttribute>> attributeDataList = ReflectionUtils.GetAttributeData<BehaviourAttribute>();
        foreach (AttributeData<BehaviourAttribute> attributeData in attributeDataList)
        {
            if (attributeData.Type.IsSubclassOf(typeof(Behaviour)))
            {
                BehaviourData.Add(new BehaviourData(attributeData));
            }
            else
            {
                Debug.LogError($"Found behaviour attribute on class that does not extend Behaviour: {attributeData.Type.Name}");
            }
        }

        SortBehaviourData();

        BehaviourDataByAction.Clear();
        foreach (AIAction action in Enum.GetValues(typeof(AIAction))) {
            BehaviourDataByAction.Add(action, GetDataByAction(action));
        }
    }

    private static void SortBehaviourData()
    {
        BehaviourData.OrderBy(d => d.Behaviour.Name);
    }

    private static List<BehaviourData> GetDataByAction(AIAction action)
    {
        return BehaviourData
            .Where(d => d.Actions.Contains(action))
            .ToList();
    }
}
