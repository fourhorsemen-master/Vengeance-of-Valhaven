using System;
using System.Linq;

[Serializable]
public class SerializableAbilityBonusLookup : SerializableDictionary<string, SerializableAbilityBonusMetadata>
{
    public bool MissingData => Values.Any(v => v.MissingData);
}
