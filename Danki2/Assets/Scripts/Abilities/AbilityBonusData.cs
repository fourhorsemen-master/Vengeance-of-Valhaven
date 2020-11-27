using System.Collections.Generic;

public class AbilityBonusData
{
    public string DisplayName { get; }
    public List<TemplatedTooltipSegment> TemplatedTooltipSegments { get; }
    public int RequiredTreeDepth { get; }

    public AbilityBonusData(string displayName, List<TemplatedTooltipSegment> templatedTooltipSegments, int requiredTreeDepth)
    {
        DisplayName = displayName;
        TemplatedTooltipSegments = templatedTooltipSegments;
        RequiredTreeDepth = requiredTreeDepth;
    }
}
