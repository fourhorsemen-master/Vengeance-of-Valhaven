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

            attributeData.AddRange(GetAttributeData<TAttribute>(assembly));
        }

        return attributeData;
    }

    public static List<AttributeData<TAttribute>> GetAttributeData<TAttribute>(Assembly assembly) where TAttribute : Attribute
    {
        List<AttributeData<TAttribute>> attributeData = new List<AttributeData<TAttribute>>();
        
        foreach (Type type in assembly.GetExportedTypes())
        {
            if (TryGetAttribute(type, out TAttribute attribute))
            {
                attributeData.Add(new AttributeData<TAttribute>(attribute, type));
            }
        }

        return attributeData;
    }

    public static List<Type> GetInheritingTypes(Type baseType, bool includeAbstractTypes = true)
    {
        List<Type> types = new List<Type>();

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.IsDynamic)
            {
                continue;
            }

            types.AddRange(GetInheritingTypes(baseType, assembly, includeAbstractTypes));
        }

        return types;
    }

    public static List<Type> GetInheritingTypes(Type baseType, Assembly assembly, bool includeAbstractTypes = true)
    {
        return assembly.GetExportedTypes()
            .Where(type => (includeAbstractTypes || !type.IsAbstract) && type.IsSubclassOf(baseType))
            .ToList();
    }

    private static bool TryGetAttribute<TAttribute>(Type type, out TAttribute attribute) where TAttribute : Attribute
    {
        attribute = type.GetCustomAttributes<TAttribute>().FirstOrDefault();
        return attribute != null;
    }
}
