using System.Collections.Generic;
using UnityEngine;

public static class PlannerScanner
{
    public static List<PlannerData> PlannerData { get; private set; } = new List<PlannerData>();

    public static void Scan()
    {
        PlannerData.Clear();
        List<AttributeData<PlannerAttribute>> attributeDataList = ReflectionUtils.GetAttributeData<PlannerAttribute>();
        foreach (AttributeData<PlannerAttribute> attributeData in attributeDataList)
        {
            if (attributeData.Type.IsSubclassOf(typeof(Planner)))
            {
                PlannerData.Add(new PlannerData(attributeData));
            }
            else
            {
                Debug.LogError($"Found planner attribute on class that does not extend Planner: {attributeData.Type.Name}");
            }
        }
    }
}
