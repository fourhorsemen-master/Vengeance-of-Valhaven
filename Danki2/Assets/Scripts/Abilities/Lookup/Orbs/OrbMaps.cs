using System;
using UnityEngine;

[Serializable]
public class OrbDisplayNameMap : SerializableEnumDictionary<OrbType, string>
{
    public OrbDisplayNameMap(string defaultValue) : base(defaultValue) { }

    public OrbDisplayNameMap(Func<string> defaultValueProvider) : base(defaultValueProvider) { }
}

[Serializable]
public class OrbColourMap : SerializableEnumDictionary<OrbType, Color>
{
    public OrbColourMap(Color defaultValue) : base(defaultValue) { }

    public OrbColourMap(Func<Color> defaultValueProvider) : base(defaultValueProvider) { }
}

[Serializable]
public class OrbSpriteMap : SerializableEnumDictionary<OrbType, Sprite>
{
    public OrbSpriteMap(Sprite defaultValue) : base(defaultValue) { }

    public OrbSpriteMap(Func<Sprite> defaultValueProvider) : base(defaultValueProvider) { }
}
