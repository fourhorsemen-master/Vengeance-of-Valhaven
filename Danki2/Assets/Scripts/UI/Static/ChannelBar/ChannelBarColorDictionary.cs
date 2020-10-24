using System;
using UnityEngine;

[Serializable]
public class ChannelBarColourDictionary : SerializableEnumDictionary<ChannelType, Color>
{
    public ChannelBarColourDictionary(Color defaultValue) : base(defaultValue)
    {
    }

    public ChannelBarColourDictionary(Func<Color> defaultValueProvider) : base(defaultValueProvider)
    {
    }
}
