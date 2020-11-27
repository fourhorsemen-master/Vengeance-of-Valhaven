using System.Collections.Generic;

public class AbilityBonusData
{
    public string DisplayName { get; }
    public List<TemplatedTooltipSegment> TemplatedTooltipSegments { get; }

    public AbilityBonusData(string displayName, List<TemplatedTooltipSegment> templatedTooltipSegments)
    {
        DisplayName = displayName;
        TemplatedTooltipSegments = templatedTooltipSegments;
    }
}
