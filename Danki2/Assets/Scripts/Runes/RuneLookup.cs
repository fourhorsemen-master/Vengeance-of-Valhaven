using System;
using UnityEngine;

[Serializable]
public class RuneData
{
    [SerializeField] private string displayName = "";
    [SerializeField] private string tooltip = "";
    [SerializeField] private Sprite sprite = null;

    public string DisplayName { get => displayName; set => displayName = value; }
    public string Tooltip { get => tooltip; set => tooltip = value; }
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
    public Sprite GetSprite(Rune rune) => runeDataLookup[rune].Sprite;
}
