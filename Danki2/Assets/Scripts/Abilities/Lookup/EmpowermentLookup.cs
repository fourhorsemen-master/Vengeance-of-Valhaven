using System;
using UnityEngine;

[Serializable]
public class EmpowermentStringDictionary : SerializableEnumDictionary<Empowerment, string>
{
    public EmpowermentStringDictionary(string defaultValue) : base(defaultValue) {}
    public EmpowermentStringDictionary(Func<string> defaultValueProvider) : base(defaultValueProvider) {}
}

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
    [SerializeField] private EmpowermentStringDictionary empowermentDisplayNameDictionary = new EmpowermentStringDictionary("");
    [SerializeField] private EmpowermentStringDictionary empowermentTooltipDictionary = new EmpowermentStringDictionary("");
    [SerializeField] private EmpowermentColourDictionary empowermentColourDictionary = new EmpowermentColourDictionary(Color.white);
    [SerializeField] private EmpowermentDecalMaterialDictionary empowermentDecalMaterialDictionary = new EmpowermentDecalMaterialDictionary(defaultValue: null);

    public EmpowermentStringDictionary EmpowermentDisplayNameDictionary => empowermentDisplayNameDictionary;
    public EmpowermentStringDictionary EmpowermentTooltipDictionary => empowermentTooltipDictionary;
    public EmpowermentColourDictionary EmpowermentColourDictionary => empowermentColourDictionary;
    public EmpowermentDecalMaterialDictionary EmpowermentDecalMaterialDictionary => empowermentDecalMaterialDictionary;

    public string GetDisplayName(Empowerment empowerment) => empowermentDisplayNameDictionary[empowerment];
    public string GetTooltip(Empowerment empowerment) => empowermentTooltipDictionary[empowerment];
    public Color GetColour(Empowerment empowerment) => empowermentColourDictionary[empowerment];
    public Material GetDecalMaterial(Empowerment empowerment) => empowermentDecalMaterialDictionary[empowerment];
}
