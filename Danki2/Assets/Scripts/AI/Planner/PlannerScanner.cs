using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlannerScanner
{
    public static List<AttributeData<PlannerAttribute>> PlannerData { get; private set; } = new List<AttributeData<PlannerAttribute>>();

    public static void Scan()
    {
        PlannerData.Clear();
        List<AttributeData<PlannerAttribute>> attributeDataList = ReflectionUtils.GetAttributeData<PlannerAttribute>();
        foreach (AttributeData<PlannerAttribute> attributeData in attributeDataList)
        {
            if (attributeData.Type.IsSubclassOf(typeof(Planner)))
            {
                PlannerData.Add(attributeData);
            }
            else
            {
                Debug.LogError($"Found planner attribute on class that does not extend Planner: {attributeData.Type.Name}");
            }
        }

        SortPlannerData();
    }

    private static void SortPlannerData()
    {
        PlannerData.OrderBy(d => d.Type.Name);
    }
}
