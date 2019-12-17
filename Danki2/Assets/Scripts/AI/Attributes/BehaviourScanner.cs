using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class BehaviourScanner
{
    public static List<BehaviourAttribute> GetBehaviours()
    {
        List<BehaviourAttribute> behaviours = new List<BehaviourAttribute>();

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.IsDynamic)
            {
                continue;
            }

            foreach (Type type in assembly.GetExportedTypes())
            {
                if (TryGetAttribute(type, out BehaviourAttribute attribute))
                {
                    behaviours.Add(attribute);
                }
            }
        }

        return behaviours;
    }

    private static bool TryGetAttribute(Type type, out BehaviourAttribute attribute)
    {
        attribute = type.GetCustomAttributes<BehaviourAttribute>().FirstOrDefault();
        return attribute != null;
    }
}
