using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ReflectionUtils
{

    public static List<AttributeData<TAttribute>> GetAttributeData<TAttribute>() where TAttribute : Attribute
    {
        List<AttributeData<TAttribute>> attributeData = new List<AttributeData<TAttribute>>();

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.IsDynamic)
            {
                continue;
            }

            foreach (Type type in assembly.GetExportedTypes())
            {
                if (TryGetAttribute(type, out TAttribute attribute))
                {
                    attributeData.Add(new AttributeData<TAttribute>(attribute, type));
                }
            }
        }

        return attributeData;
    }

    private static bool TryGetAttribute<TAttribute>(Type type, out TAttribute attribute) where TAttribute : Attribute
    {
        attribute = type.GetCustomAttributes<TAttribute>().FirstOrDefault();
        return attribute != null;
    }
}
