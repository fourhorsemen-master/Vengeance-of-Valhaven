using System;
using System.Collections.Generic;
using System.Linq;

public static class PlayerListTooltipBuilder
{
    public static List<TooltipSegment> Build(AbilityReference ability)
    {
        List<TemplatedTooltipSegment> templatedTooltipSegments = AbilityLookup.Instance.GetTemplatedTooltipSegments(ability);

        return templatedTooltipSegments
            .Select(templatedTooltipSegment => GetTooltipSegment(
                templatedTooltipSegment,
                AbilityLookup.Instance.GetBaseAbilityData(ability)
            ))
            .ToList();
    }

    private static TooltipSegment GetTooltipSegment(
        TemplatedTooltipSegment templatedTooltipSegment,
        AbilityData baseAbilityData
    )
    {
        switch (templatedTooltipSegment.Type)
        {
            case TemplatedTooltipSegmentType.Text:
                return GetTextSegment(templatedTooltipSegment);

            case TemplatedTooltipSegmentType.PrimaryDamage:
                return GetNumericTooltipSegment(baseAbilityData.PrimaryDamage);

            case TemplatedTooltipSegmentType.SecondaryDamage:
                return GetNumericTooltipSegment(baseAbilityData.SecondaryDamage);

            case TemplatedTooltipSegmentType.Heal:
                return GetNumericTooltipSegment(baseAbilityData.Heal);

            case TemplatedTooltipSegmentType.Shield:
                return GetNumericTooltipSegment(baseAbilityData.Shield);

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static TooltipSegment GetTextSegment(TemplatedTooltipSegment templatedTooltipSegment)
    {
        return new TooltipSegment(TooltipSegmentType.Text, templatedTooltipSegment.Value);
    }

    private static TooltipSegment GetNumericTooltipSegment(int baseNumericValue)
    {
        return new TooltipSegment(TooltipSegmentType.UnaffectedNumericValue, baseNumericValue.ToString());
    }
}
