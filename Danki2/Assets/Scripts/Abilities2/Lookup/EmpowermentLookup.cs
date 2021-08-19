using System;
using UnityEngine;

[Serializable]
public class EmpowermentColourDictionary : SerializableEnumDictionary<Empowerment, Color>
{
    public EmpowermentColourDictionary(Color defaultValue) : base(defaultValue) {}
    public EmpowermentColourDictionary(Func<Color> defaultValueProvider) : base(defaultValueProvider) {}
}

public class EmpowermentLookup : Singleton<EmpowermentLookup>
{
    public EmpowermentColourDictionary empowermentColourDictionary = new EmpowermentColourDictionary(Color.white);

    public Color GetColour(Empowerment empowerment) => empowermentColourDictionary[empowerment];
}
