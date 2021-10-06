using System;
using UnityEngine;

[Serializable]
public class EmpowermentColourDictionary : SerializableEnumDictionary<Empowerment, Color>
{
    public EmpowermentColourDictionary(Color defaultValue) : base(defaultValue) {}
    public EmpowermentColourDictionary(Func<Color> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class EmpowermentDecalMaterialDictionary : SerializableEnumDictionary<Empowerment, Material>
{
    public EmpowermentDecalMaterialDictionary(Material defaultValue) : base(defaultValue) { }
    public EmpowermentDecalMaterialDictionary(Func<Material> defaultValueProvider) : base(defaultValueProvider) { }
}

public class EmpowermentLookup : Singleton<EmpowermentLookup>
{
    public EmpowermentColourDictionary empowermentColourDictionary = new EmpowermentColourDictionary(Color.white);

    public EmpowermentDecalMaterialDictionary empowermentDecalMaterialDictionary = new EmpowermentDecalMaterialDictionary(() => null);

    public Color GetColour(Empowerment empowerment) => empowermentColourDictionary[empowerment];
    public Material GetDecalMaterial(Empowerment empowerment) => empowermentDecalMaterialDictionary[empowerment];
}
