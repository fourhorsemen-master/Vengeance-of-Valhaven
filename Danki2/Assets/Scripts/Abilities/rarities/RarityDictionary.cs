using System;

[Serializable]
public class RarityDictionary : SerializableEnumDictionary<Rarity, RarityData> {
    public RarityDictionary(RarityData defaultValue) : base(defaultValue) { }
    public RarityDictionary(Func<RarityData> defaultValueProvider) : base(defaultValueProvider) { }
}
