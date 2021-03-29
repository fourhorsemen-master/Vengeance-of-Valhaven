using UnityEngine;

public class TooltipLookup : Singleton<TooltipLookup>
{
    [SerializeField] private AbilityTooltip abilityTooltipPrefab = null;

    public AbilityTooltip AbilityTooltipPrefab => abilityTooltipPrefab;
}
