using System;
using System.Collections.Generic;
using System.Linq;

public static class PlayerListTooltipBuilder
{
    public static List<TooltipSegment> Build(AbilityReference ability)
    {
        List<TemplatedTooltipSegment> templatedTooltipSegments = AbilityLookup.Instance.GetTemplatedTooltipSegments(ability);

        bool hasOrbType = AbilityLookup.Instance.TryGetAbilityOrbType(ability, out _);

        return templatedTooltipSegments
            .SelectMany(templatedTooltipSegment => GetTooltipSegments(
                templatedTooltipSegment,
                AbilityLookup.Instance.GetBaseAbilityData(ability),
                hasOrbType
            ))
            .ToList();
    }

    private static List<TooltipSegment> GetTooltipSegments(
        TemplatedTooltipSegment templatedTooltipSegment,
        AbilityData baseAbilityData,
        bool hasOrbType
    )
    {
        switch (templatedTooltipSegment.Type)
        {
            case TemplatedTooltipSegmentType.Text:
                return GetTextSegments(templatedTooltipSegment);

            case TemplatedTooltipSegmentType.PrimaryDamage:
                return GetNumericTooltipSegments(baseAbilityData.PrimaryDamage, hasOrbType);

            case TemplatedTooltipSegmentType.SecondaryDamage:
                return GetNumericTooltipSegments(baseAbilityData.SecondaryDamage, hasOrbType);

            case TemplatedTooltipSegmentType.Heal:
                return GetNumericTooltipSegments(baseAbilityData.Heal, hasOrbType);

            case TemplatedTooltipSegmentType.Shield:
                return GetNumericTooltipSegments(baseAbilityData.Shield, hasOrbType);

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static List<TooltipSegment> GetTextSegments(TemplatedTooltipSegment templatedTooltipSegment)
    {
        return ListUtils.Singleton(new TooltipSegment(TooltipSegmentType.Text, templatedTooltipSegment.Value));
    }

    private static List<TooltipSegment> GetNumericTooltipSegments(int baseNumericValue, bool hasOrbType)
    {
        List<TooltipSegment> tooltipSegments = ListUtils.Singleton(
            new TooltipSegment(TooltipSegmentType.BaseNumericValue, baseNumericValue.ToString())
        );

        if (hasOrbType)
        {
            tooltipSegments.Add(new TooltipSegment(TooltipSegmentType.BonusNumericValue, "1/orb"));
        }

        return tooltipSegments;
    }
}
