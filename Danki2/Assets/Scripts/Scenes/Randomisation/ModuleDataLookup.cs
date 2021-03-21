using System;

[Serializable]
public class ModuleDataLookup : SerializableEnumDictionary<SocketType, SocketData>
{
    public ModuleDataLookup(SocketData defaultValue) : base(defaultValue) {}
    public ModuleDataLookup(Func<SocketData> defaultValueProvider) : base(defaultValueProvider) {}
}
