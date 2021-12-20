using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EmpowermentStringDictionary : SerializableEnumDictionary<Empowerment, string>
{
    public EmpowermentStringDictionary(string defaultValue) : base(defaultValue) {}
    public EmpowermentStringDictionary(Func<string> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class ActiveEffectListWrapper
{
    [SerializeField] private List<ActiveEffect> activeEffects = new List<ActiveEffect>();
    public List<ActiveEffect> ActiveEffects => activeEffects;
}

[Serializable]
public class EmpowermentActiveEffectsDictionary : SerializableEnumDictionary<Empowerment, ActiveEffectListWrapper>
{
    public EmpowermentActiveEffectsDictionary(ActiveEffectListWrapper defaultValue) : base(defaultValue) {}
    public EmpowermentActiveEffectsDictionary(Func<ActiveEffectListWrapper> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class PassiveEffectListWrapper
{
    [SerializeField] private List<PassiveEffect> passiveEffects = new List<PassiveEffect>();
    public List<PassiveEffect> PassiveEffects => passiveEffects;
}

[Serializable]
public class EmpowermentPassiveEffectsDictionary : SerializableEnumDictionary<Empowerment, PassiveEffectListWrapper>
{
    public EmpowermentPassiveEffectsDictionary(PassiveEffectListWrapper defaultValue) : base(defaultValue) {}
    public EmpowermentPassiveEffectsDictionary(Func<PassiveEffectListWrapper> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class StackingEffectListWrapper
{
    [SerializeField] private List<StackingEffect> stackingEffects = new List<StackingEffect>();
    public List<StackingEffect> StackingEffects => stackingEffects;
}

[Serializable]
public class EmpowermentStackingEffectsDictionary : SerializableEnumDictionary<Empowerment, StackingEffectListWrapper>
{
    public EmpowermentStackingEffectsDictionary(StackingEffectListWrapper defaultValue) : base(defaultValue) {}
    public EmpowermentStackingEffectsDictionary(Func<StackingEffectListWrapper> defaultValueProvider) : base(defaultValueProvider) {}
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

[Serializable]
public class EmpowermentSpriteDictionary : SerializableEnumDictionary<Empowerment, Sprite>
{
    public EmpowermentSpriteDictionary(Sprite defaultValue) : base(defaultValue) { }
    public EmpowermentSpriteDictionary(Func<Sprite> defaultValueProvider) : base(defaultValueProvider) { }
}

public class EmpowermentLookup : Singleton<EmpowermentLookup>
{
    [SerializeField] private EmpowermentStringDictionary empowermentDisplayNameDictionary = new EmpowermentStringDictionary("");
    [SerializeField] private EmpowermentStringDictionary empowermentTooltipDictionary = new EmpowermentStringDictionary("");
    [SerializeField] private EmpowermentActiveEffectsDictionary empowermentActiveEffectsDictionary = new EmpowermentActiveEffectsDictionary(() => new ActiveEffectListWrapper());
    [SerializeField] private EmpowermentPassiveEffectsDictionary empowermentPassiveEffectsDictionary = new EmpowermentPassiveEffectsDictionary(() => new PassiveEffectListWrapper());
    [SerializeField] private EmpowermentStackingEffectsDictionary empowermentStackingEffectsDictionary = new EmpowermentStackingEffectsDictionary(() => new StackingEffectListWrapper());
    [SerializeField] private EmpowermentColourDictionary empowermentColourDictionary = new EmpowermentColourDictionary(Color.white);
    [SerializeField] private EmpowermentDecalMaterialDictionary empowermentDecalMaterialDictionary = new EmpowermentDecalMaterialDictionary(defaultValue: null);
    [SerializeField] private EmpowermentSpriteDictionary empowermentSpriteDictionary = new EmpowermentSpriteDictionary(defaultValue: null);

    public EmpowermentStringDictionary EmpowermentDisplayNameDictionary => empowermentDisplayNameDictionary;
    public EmpowermentStringDictionary EmpowermentTooltipDictionary => empowermentTooltipDictionary;
    public EmpowermentActiveEffectsDictionary EmpowermentActiveEffectsDictionary => empowermentActiveEffectsDictionary;
    public EmpowermentPassiveEffectsDictionary EmpowermentPassiveEffectsDictionary => empowermentPassiveEffectsDictionary;
    public EmpowermentStackingEffectsDictionary EmpowermentStackingEffectsDictionary => empowermentStackingEffectsDictionary;
    public EmpowermentColourDictionary EmpowermentColourDictionary => empowermentColourDictionary;
    public EmpowermentDecalMaterialDictionary EmpowermentDecalMaterialDictionary => empowermentDecalMaterialDictionary;
    public EmpowermentSpriteDictionary EmpowermentSpriteDictionary => empowermentSpriteDictionary;

    public string GetDisplayName(Empowerment empowerment) => empowermentDisplayNameDictionary[empowerment];
    public string GetTooltip(Empowerment empowerment) => empowermentTooltipDictionary[empowerment];
    public List<ActiveEffect> GetActiveEffects(Empowerment empowerment) => empowermentActiveEffectsDictionary[empowerment].ActiveEffects;
    public List<PassiveEffect> GetPassiveEffects(Empowerment empowerment) => empowermentPassiveEffectsDictionary[empowerment].PassiveEffects;
    public List<StackingEffect> GetStackingEffects(Empowerment empowerment) => empowermentStackingEffectsDictionary[empowerment].StackingEffects;
    public Color GetColour(Empowerment empowerment) => empowermentColourDictionary[empowerment];
    public Material GetDecalMaterial(Empowerment empowerment) => empowermentDecalMaterialDictionary[empowerment];
    public Sprite GetSprite(Empowerment empowerment) => empowermentSpriteDictionary[empowerment];
}
