using System;
using System.Collections.Generic;

public static class TooltipBuilderUtils
{
    /// <summary>
    /// Builds the given templated tooltip segment into a tooltip segment.
    /// </summary>
    public static TooltipSegment GetTooltipSegment(TemplatedTooltipSegment templatedTooltipSegment)
    {
        return templatedTooltipSegment.Type switch
        {
            TemplatedTooltipSegmentType.Text => new TooltipSegment(TooltipSegmentType.Text, templatedTooltipSegment.Value),
            TemplatedTooltipSegmentType.Heal => new TooltipSegment(TooltipSegmentType.BoldText, "Heal"),
            TemplatedTooltipSegmentType.Stun => new TooltipSegment(TooltipSegmentType.BoldText, "Stun"),
            TemplatedTooltipSegmentType.Slow => new TooltipSegment(TooltipSegmentType.BoldText, "Slow"),
            TemplatedTooltipSegmentType.Block => new TooltipSegment(TooltipSegmentType.BoldText, "Block"),
            TemplatedTooltipSegmentType.Bleed => new TooltipSegment(TooltipSegmentType.BoldText, "Bleed"),
            TemplatedTooltipSegmentType.Poison => new TooltipSegment(TooltipSegmentType.BoldText, "Poison"),
            TemplatedTooltipSegmentType.Vulnerable => new TooltipSegment(TooltipSegmentType.BoldText, "Vulnerable"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
