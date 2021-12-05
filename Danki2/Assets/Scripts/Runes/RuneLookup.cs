using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RuneData
{
    [SerializeField] private string displayName = "";
    [SerializeField] private string tooltip = "";
    [SerializeField] private List<ActiveEffect> activeEffects = new List<ActiveEffect>();
    [SerializeField] private List<PassiveEffect> passiveEffects = new List<PassiveEffect>();
    [SerializeField] private List<StackingEffect> stackingEffects = new List<StackingEffect>();
    [SerializeField] private Sprite sprite = null;

    public string DisplayName { get => displayName; set => displayName = value; }
    public string Tooltip { get => tooltip; set => tooltip = value; }
    public List<ActiveEffect> ActiveEffects => activeEffects;
    public List<PassiveEffect> PassiveEffects => passiveEffects;
    public List<StackingEffect> StackingEffects => stackingEffects;
    public Sprite Sprite { get => sprite; set => sprite = value; }
}

[Serializable]
public class RuneDataLookup : SerializableEnumDictionary<Rune, RuneData>
{
    public RuneDataLookup(RuneData defaultValue) : base(defaultValue) {}
    public RuneDataLookup(Func<RuneData> defaultValueProvider) : base(defaultValueProvider) {}
}

public class RuneLookup : Singleton<RuneLookup>
{
    [SerializeField] public RuneDataLookup runeDataLookup = new RuneDataLookup(() => new RuneData());

    public string GetDisplayName(Rune rune) => runeDataLookup[rune].DisplayName;
    public string GetTooltip(Rune rune) => runeDataLookup[rune].Tooltip;
    public List<ActiveEffect> GetActiveEffects(Rune rune) => runeDataLookup[rune].ActiveEffects;
    public List<PassiveEffect> GetPassiveEffects(Rune rune) => runeDataLookup[rune].PassiveEffects;
    public List<StackingEffect> GetStackingEffects(Rune rune) => runeDataLookup[rune].StackingEffects;
    public Sprite GetSprite(Rune rune) => runeDataLookup[rune].Sprite;
}
