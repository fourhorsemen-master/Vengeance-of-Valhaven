using System.Collections.Generic;

public class AbilityBonusData
{
    public string DisplayName { get; }
    public string Tooltip { get; }
    public Dictionary<OrbType, int> RequiredOrbs { get; }

    public AbilityBonusData(string displayName, string tooltip, Dictionary<OrbType, int> requiredOrbs)
    {
        DisplayName = displayName;
        Tooltip = tooltip;
        RequiredOrbs = requiredOrbs;
    }
}
