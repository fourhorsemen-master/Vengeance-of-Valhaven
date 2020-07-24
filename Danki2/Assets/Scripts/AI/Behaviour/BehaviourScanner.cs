using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BehaviourScanner
{
    public static List<AttributeData<BehaviourAttribute>> BehaviourData { get; private set; } = new List<AttributeData<BehaviourAttribute>>();
    public static Dictionary<AIAction, List<AttributeData<BehaviourAttribute>>> BehaviourDataByAction { get; private set; } = new Dictionary<AIAction, List<AttributeData<BehaviourAttribute>>>();

    public static void Scan()
    {
        BehaviourData.Clear();
        List<AttributeData<BehaviourAttribute>> attributeDataList = ReflectionUtils.GetAttributeData<BehaviourAttribute>();
        foreach (AttributeData<BehaviourAttribute> attributeData in attributeDataList)
        {
            if (attributeData.Type.IsSubclassOf(typeof(Behaviour)))
            {
                BehaviourData.Add(attributeData);
            }
            else
            {
                Debug.LogError($"Found behaviour attribute on class that does not extend Behaviour: {attributeData.Type.Name}");
            }
        }

        SortBehaviourData();

        BehaviourDataByAction.Clear();
        EnumUtils.ForEach<AIAction>(action =>
        {
            BehaviourDataByAction.Add(action, GetDataByAction(action));
        });
    }

    private static void SortBehaviourData()
    {
        BehaviourData.OrderBy(d => d.Type.Name);
    }

    private static List<AttributeData<BehaviourAttribute>> GetDataByAction(AIAction action)
    {
        return BehaviourData
            .Where(d => d.Attribute.Actions.Contains(action))
            .ToList();
    }
}
