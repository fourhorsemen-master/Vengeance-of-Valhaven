using System;

[Serializable]
public class SerializableMetadataLookup : SerializableEnumDictionary<AbilityReference, SerializableAbilityMetadata>
{
    public SerializableMetadataLookup(SerializableAbilityMetadata defaultValue) : base(defaultValue)
    {
    }

    public SerializableMetadataLookup(SerializableEnumDictionary<AbilityReference, SerializableAbilityMetadata> dictionary) : base(dictionary)
    {
    }

    public SerializableMetadataLookup(Func<SerializableAbilityMetadata> defaultValueProvider) : base(defaultValueProvider)
    {
    }
}
