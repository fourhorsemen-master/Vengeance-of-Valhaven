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

    public static List<Type> GetInheritingTypes(Type baseType, params ClassModifier[] modifiersToIgnore)
    {
        List<Type> types = new List<Type>();

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.IsDynamic)
            {
                continue;
            }

            types.AddRange(GetInheritingTypes(baseType, assembly, modifiersToIgnore));
        }

        return types;
    }

    public static List<Type> GetInheritingTypes(Type baseType, Assembly assembly, params ClassModifier[] modifiersToIgnore)
    {
        return assembly.GetExportedTypes()
            .Where(type =>
            {
                if (modifiersToIgnore.Contains(ClassModifier.Abstract) && type.IsAbstract) return false;
                if (modifiersToIgnore.Contains(ClassModifier.Sealed) && type.IsSealed) return false;
                return type.IsSubclassOf(baseType);
            })
            .ToList();
    }

    private static bool TryGetAttribute<TAttribute>(Type type, out TAttribute attribute) where TAttribute : Attribute
    {
        attribute = type.GetCustomAttributes<TAttribute>().FirstOrDefault();
        return attribute != null;
    }
}
