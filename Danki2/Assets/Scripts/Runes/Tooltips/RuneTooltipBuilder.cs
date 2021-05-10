using System.Collections.Generic;
using System.Linq;

public static class RuneTooltipBuilder
{
    public static List<TooltipSegment> Build(Rune rune)
    {
        List<TemplatedTooltipSegment> templatedTooltipSegments = RuneLookup.Instance.GetTemplatedTooltipSegments(rune);

        return templatedTooltipSegments.Select(templatedSegment =>
            TooltipBuilderUtils.GetTooltipSegment(templatedSegment)
        ).ToList();
    }
}
