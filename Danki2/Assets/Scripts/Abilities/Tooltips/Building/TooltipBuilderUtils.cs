using System;
using System.Collections.Generic;

public static class TooltipBuilderUtils
{
    private static readonly ISet<TemplatedTooltipSegmentType> HandleableTypes = new HashSet<TemplatedTooltipSegmentType>
    {
        TemplatedTooltipSegmentType.Text,
        TemplatedTooltipSegmentType.Stun,
        TemplatedTooltipSegmentType.Slow,
        TemplatedTooltipSegmentType.Block,
        TemplatedTooltipSegmentType.Bleed,
        TemplatedTooltipSegmentType.Poison,
        TemplatedTooltipSegmentType.Vulnerable,
        TemplatedTooltipSegmentType.BleedInfo,
    };
    
    /// <summary>
    /// Returns true if the given templated tooltip segment can be turned into a tooltip segment by this class.
    /// </summary>
    public static bool CanHandle(TemplatedTooltipSegment templatedTooltipSegment)
    {
        return HandleableTypes.Contains(templatedTooltipSegment.Type);
    }

    /// <summary>
    /// Builds the given templated tooltip segment into a tooltip segment. Only certain types of templated
    /// tooltip segments will work, so the type must be checked first with the method above.
    /// </summary>
    public static TooltipSegment GetTooltipSegment(TemplatedTooltipSegment templatedTooltipSegment)
    {
        switch (templatedTooltipSegment.Type)
        {
            case TemplatedTooltipSegmentType.Text:
                return new TooltipSegment(TooltipSegmentType.Text, templatedTooltipSegment.Value);
            
            case TemplatedTooltipSegmentType.Stun:
                return new TooltipSegment(TooltipSegmentType.BoldText, "Stun");

            case TemplatedTooltipSegmentType.Slow:
                return new TooltipSegment(TooltipSegmentType.BoldText, "Slow");
            
            case TemplatedTooltipSegmentType.Block:
                return new TooltipSegment(TooltipSegmentType.BoldText, "Block");
            
            case TemplatedTooltipSegmentType.Bleed:
                return new TooltipSegment(TooltipSegmentType.BoldText, $"Bleed x{templatedTooltipSegment.Value}");
            
            case TemplatedTooltipSegmentType.Poison:
                return new TooltipSegment(TooltipSegmentType.BoldText, "Poison");
            
            case TemplatedTooltipSegmentType.Vulnerable:
                return new TooltipSegment(TooltipSegmentType.BoldText, $"Vulnerable x{templatedTooltipSegment.Value}");
            
            case TemplatedTooltipSegmentType.BleedInfo:
                return new TooltipSegment(TooltipSegmentType.BoldText, "Bleed");

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
