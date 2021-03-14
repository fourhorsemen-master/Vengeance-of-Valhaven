using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nested lists aren't serialisable by default, adding this wrapper is a workaround.
/// </summary>
[Serializable]
public class ModuleDataListWrapper
{
    [SerializeField] private List<ModuleData> list = new List<ModuleData>();

    public List<ModuleData> List => list;
}

[Serializable]
public class ModuleDataLookup : SerializableEnumDictionary<SocketType, ModuleDataListWrapper>
{
    public ModuleDataLookup(ModuleDataListWrapper defaultValue) : base(defaultValue) {}
    public ModuleDataLookup(Func<ModuleDataListWrapper> defaultValueProvider) : base(defaultValueProvider) {}
}
