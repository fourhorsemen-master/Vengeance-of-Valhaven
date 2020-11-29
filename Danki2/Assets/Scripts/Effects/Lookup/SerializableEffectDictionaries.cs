using System;

[Serializable]
public class SerializableActiveEffectDictionary : SerializableEnumDictionary<ActiveEffect, SerializableActiveEffectData>
{
    public SerializableActiveEffectDictionary(SerializableActiveEffectData defaultValue) : base(defaultValue) {}
    public SerializableActiveEffectDictionary(Func<SerializableActiveEffectData> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class SerializablePassiveEffectDictionary : SerializableEnumDictionary<PassiveEffect, SerializablePassiveEffectData>
{
    public SerializablePassiveEffectDictionary(SerializablePassiveEffectData defaultValue) : base(defaultValue) {}
    public SerializablePassiveEffectDictionary(Func<SerializablePassiveEffectData> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class SerializableStackingEffectDictionary : SerializableEnumDictionary<StackingEffect, SerializableStackingEffectData>
{
    public SerializableStackingEffectDictionary(SerializableStackingEffectData defaultValue) : base(defaultValue) {}
    public SerializableStackingEffectDictionary(Func<SerializableStackingEffectData> defaultValueProvider) : base(defaultValueProvider) {}
}
