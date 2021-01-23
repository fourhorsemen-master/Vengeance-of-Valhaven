using System;
using System.Collections.Generic;

public static class TooltipBuilderUtils
{
    private static readonly ISet<TemplatedTooltipSegmentType> HandleableTypes = new HashSet<TemplatedTooltipSegmentType>
    {
        TemplatedTooltipSegmentType.Text,
        TemplatedTooltipSegmentType.Stun,
        TemplatedTooltipSegmentType.PassiveSlow,
        TemplatedTooltipSegmentType.Block,
        TemplatedTooltipSegmentType.StackingSlow,
        TemplatedTooltipSegmentType.Bleed,
        TemplatedTooltipSegmentType.Poison,
        TemplatedTooltipSegmentType.Vulnerable,
    };
    
    public static bool CanHandle(TemplatedTooltipSegment templatedTooltipSegment)
    {
        return HandleableTypes.Contains(templatedTooltipSegment.Type);
    }

    public static TooltipSegment GetTooltipSegment(TemplatedTooltipSegment templatedTooltipSegment)
    {
        switch (templatedTooltipSegment.Type)
        {
            case TemplatedTooltipSegmentType.Text:
                return new TooltipSegment(TooltipSegmentType.Text, templatedTooltipSegment.Value);
            
            case TemplatedTooltipSegmentType.Stun:
                return new TooltipSegment(TooltipSegmentType.BoldText, "Stun");
            
            case TemplatedTooltipSegmentType.PassiveSlow:
                return new TooltipSegment(TooltipSegmentType.BoldText, "Passive Slow");
            
            case TemplatedTooltipSegmentType.Block:
                return new TooltipSegment(TooltipSegmentType.BoldText, "Block");
            
            case TemplatedTooltipSegmentType.StackingSlow:
                return new TooltipSegment(TooltipSegmentType.BoldText, $"Slow x{templatedTooltipSegment.Value}");
            
            case TemplatedTooltipSegmentType.Bleed:
                return new TooltipSegment(TooltipSegmentType.BoldText, $"Bleed x{templatedTooltipSegment.Value}");
            
            case TemplatedTooltipSegmentType.Poison:
                return new TooltipSegment(TooltipSegmentType.BoldText, $"Poison x{templatedTooltipSegment.Value}");
            
            case TemplatedTooltipSegmentType.Vulnerable:
                return new TooltipSegment(TooltipSegmentType.BoldText, $"Vulnerable x{templatedTooltipSegment.Value}");

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
