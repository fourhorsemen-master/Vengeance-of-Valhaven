using UnityEngine;

public class TooltipLookup : Singleton<TooltipLookup>
{
    [SerializeField] private AbilityTooltip abilityTooltipPrefab = null;
    [SerializeField] private RuneTooltip runeTooltip = null;

    public AbilityTooltip AbilityTooltipPrefab => abilityTooltipPrefab;
    public RuneTooltip RuneTooltip => runeTooltip;
}
