using System;
using System.Linq;

[Serializable]
public class SerializableAbilityBonusLookup : SerializableDictionary<string, SerializableAbilityBonusMetadata>
{
    public bool Valid => Values.All(v => v.Valid);
}
