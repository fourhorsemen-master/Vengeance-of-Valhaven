using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class BehaviourScanner
{
    public static List<AttributeData<BehaviourAttribute>> BehaviourData { get; private set; } = new List<AttributeData<BehaviourAttribute>>();

    public static void Scan()
    {
        BehaviourData = ReflectionUtils.GetAttributeData<BehaviourAttribute>();
    }

    public static List<string> GetValues()
    {
        return (
            from AttributeData<BehaviourAttribute> behaviourData
            in BehaviourData
            select behaviourData.Attribute.SomeValue
        ).ToList();
    }

    public static List<string> GetValuesByType(Type type)
    {
        return (
            from AttributeData<BehaviourAttribute> behaviourData
            in BehaviourData
            where behaviourData.Type.Equals(type)
            select behaviourData.Attribute.SomeValue
        ).ToList();
    }
}
