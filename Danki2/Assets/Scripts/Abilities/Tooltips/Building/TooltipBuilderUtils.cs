using System;
using System.Collections.Generic;

public static class TooltipBuilderUtils
{
    private static readonly ISet<TemplatedTooltipSegmentType> HandleableTypes = new HashSet<TemplatedTooltipSegmentType>
    {
        TemplatedTooltipSegmentType.Text,
        TemplatedTooltipSegmentType.Heal,
        TemplatedTooltipSegmentType.Stun,
        TemplatedTooltipSegmentType.Slow,
        TemplatedTooltipSegmentType.Block,
        TemplatedTooltipSegmentType.Bleed,
        TemplatedTooltipSegmentType.Poison,
        TemplatedTooltipSegmentType.Vulnerable
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
        var hasArgument = !string.IsNullOrEmpty(templatedTooltipSegment.Value);

        return templatedTooltipSegment.Type switch
        {
            TemplatedTooltipSegmentType.Text => new TooltipSegment(TooltipSegmentType.Text, templatedTooltipSegment.Value),
            TemplatedTooltipSegmentType.Heal => new TooltipSegment(TooltipSegmentType.BoldText, "Heal"),
            TemplatedTooltipSegmentType.Stun => new TooltipSegment(TooltipSegmentType.BoldText, "Stun"),
            TemplatedTooltipSegmentType.Slow => new TooltipSegment(TooltipSegmentType.BoldText, "Slow"),
            TemplatedTooltipSegmentType.Block => new TooltipSegment(TooltipSegmentType.BoldText, "Block"),
            TemplatedTooltipSegmentType.Bleed => new TooltipSegment(TooltipSegmentType.BoldText, "Bleed" + ArgumentAsMultiplier(templatedTooltipSegment)),
            TemplatedTooltipSegmentType.Poison => new TooltipSegment(TooltipSegmentType.BoldText, "Poison"),
            TemplatedTooltipSegmentType.Vulnerable => new TooltipSegment(TooltipSegmentType.BoldText, "Vulnerable" + ArgumentAsMultiplier(templatedTooltipSegment)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static string ArgumentAsMultiplier(TemplatedTooltipSegment templatedTooltipSegment)
    {
        return string.IsNullOrEmpty(templatedTooltipSegment.Value)
            ? ""
            : $" x{ templatedTooltipSegment.Value }";
    }
}
