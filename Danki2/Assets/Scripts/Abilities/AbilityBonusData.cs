using System.Collections.Generic;

public class AbilityBonusData
{
    public string DisplayName { get; }
    public List<TemplatedTooltipSegment> TemplatedTooltipSegments { get; }
    public OrbCollection RequiredOrbs { get; }
    public int RequiredTreeDepth { get; }

    public AbilityBonusData(string displayName, List<TemplatedTooltipSegment> templatedTooltipSegments, OrbCollection requiredOrbs, int requiredTreeDepth)
    {
        DisplayName = displayName;
        TemplatedTooltipSegments = templatedTooltipSegments;
        RequiredOrbs = requiredOrbs;
        RequiredTreeDepth = requiredTreeDepth;
    }
}
