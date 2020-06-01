using System;
using UnityEngine;

[Serializable]
public class AbilityIconDictionary : SerializableEnumDictionary<AbilityReference, Sprite>
{
    public AbilityIconDictionary(Sprite defaultValue) : base(defaultValue)
    {
    }

    public AbilityIconDictionary(Func<Sprite> defaultValueProvider) : base(defaultValueProvider)
    {
    }
}
