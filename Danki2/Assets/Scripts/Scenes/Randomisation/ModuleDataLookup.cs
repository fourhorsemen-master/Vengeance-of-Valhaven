using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModuleDataListWrapper
{
    [SerializeField] private List<ModuleData> list;

    public List<ModuleData> List => list;
}

[Serializable]
public class ModuleDataLookup : SerializableEnumDictionary<SocketType, ModuleDataListWrapper>
{
    public ModuleDataLookup(ModuleDataListWrapper defaultValue) : base(defaultValue) {}
    public ModuleDataLookup(Func<ModuleDataListWrapper> defaultValueProvider) : base(defaultValueProvider) {}
}
