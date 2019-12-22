using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BehaviourScanner
{
    public static List<BehaviourData> BehaviourData { get; private set; } = new List<BehaviourData>();

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
    }

    public static List<BehaviourData> GetDataByAction(AIAction action)
    {
        return (
            from BehaviourData data
            in BehaviourData
            where data.Action.Equals(action)
            select data
        ).ToList();
    }
}
