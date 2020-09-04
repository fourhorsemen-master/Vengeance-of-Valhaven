using System.Collections.Generic;

public class AbilityBonusData
{
    public string DisplayName { get; }
    public List<TemplatedTooltipSegment> TemplatedTooltipSegments { get; }
    public OrbCollection RequiredOrbs { get; }

    public AbilityBonusData(string displayName, List<TemplatedTooltipSegment> templatedTooltipSegments, OrbCollection requiredOrbs)
    {
        DisplayName = displayName;
        TemplatedTooltipSegments = templatedTooltipSegments;
        RequiredOrbs = requiredOrbs;
    }
}
