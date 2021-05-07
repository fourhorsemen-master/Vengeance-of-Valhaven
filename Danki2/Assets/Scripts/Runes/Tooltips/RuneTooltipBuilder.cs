using System.Collections.Generic;

public static class RuneTooltipBuilder
{
    public static List<TooltipSegment> Build(Rune rune)
    {
        List<TemplatedTooltipSegment> templatedTooltipSegments = RuneLookup.Instance.GetTemplatedTooltipSegments(rune);

        List<TooltipSegment> tooltipSegments = new List<TooltipSegment>();
        templatedTooltipSegments.ForEach(templatedTooltipSegment =>
        {
            tooltipSegments.Add(TooltipBuilderUtils.GetTooltipSegment(templatedTooltipSegment));
        });

        return tooltipSegments;
    }
}
