using System.Collections.Generic;
using System.Linq;

public static class BehaviourScanner
{
    public static List<BehaviourData> BehaviourData { get; private set; } = new List<BehaviourData>();

    public static void Scan()
    {
        BehaviourData = (
            from attributeData
            in ReflectionUtils.GetAttributeData<BehaviourAttribute>()
            where attributeData.Type.GetInterfaces().Contains(typeof(Behaviour))
            select new BehaviourData(attributeData)
        ).ToList();
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
