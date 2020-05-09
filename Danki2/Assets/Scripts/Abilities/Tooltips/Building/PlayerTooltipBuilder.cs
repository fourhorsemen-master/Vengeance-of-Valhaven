using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerTooltipBuilder
{
    private readonly List<IAbilityDataDiffer> differs = new List<IAbilityDataDiffer>();

    public PlayerTooltipBuilder(Player player)
    {
        differs.Add(new AbilityDataStatsDiffer(player));
        differs.Add(new AbilityDataOrbsDiffer(player.AbilityTree));
    }

    /// <summary>
    /// Builds the tooltip for the given node returning a list of tooltip segments. The tooltip segments represent
    /// the tooltip with all values bound in, so that it can be easily rendered.
    /// </summary>
    /// <param name="node"> The node to build the tooltip for </param>
    /// <returns> The build tooltip segments </returns>
    public List<TooltipSegment> Build(Node node)
    {
        AbilityReference abilityReference = node.Ability;
        List<TemplatedTooltipSegment> templatedTooltipSegments = ParsedTooltipLookup.GetTemplatedTooltipSegments(abilityReference);

        return templatedTooltipSegments
            .SelectMany(templatedTooltipSegment => GetTooltipSegments(
                templatedTooltipSegment,
                AbilityLookup.GetBaseAbilityData(abilityReference),
                AbilityData.FromAbilityDataDiffers(differs, node)
            ))
            .ToList();
    }

    private List<TooltipSegment> GetTooltipSegments(
        TemplatedTooltipSegment templatedTooltipSegment,
        AbilityData baseAbilityData,
        AbilityData abilityDataDiff
    )
    {
        switch (templatedTooltipSegment.Type)
        {
            case TemplatedTooltipSegmentType.Text:
                return GetTextSegments(templatedTooltipSegment);

            case TemplatedTooltipSegmentType.PrimaryDamage:
                return GetNumericTooltipSegments(baseAbilityData.PrimaryDamage, abilityDataDiff.PrimaryDamage);

            case TemplatedTooltipSegmentType.SecondaryDamage:
                return GetNumericTooltipSegments(baseAbilityData.SecondaryDamage, abilityDataDiff.SecondaryDamage);

            case TemplatedTooltipSegmentType.Heal:
                return GetNumericTooltipSegments(baseAbilityData.Heal, abilityDataDiff.Heal);

            case TemplatedTooltipSegmentType.Shield:
                return GetNumericTooltipSegments(baseAbilityData.Shield, abilityDataDiff.Shield);

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private List<TooltipSegment> GetTextSegments(TemplatedTooltipSegment templatedTooltipSegment)
    {
        return ListUtils.Singleton(new TooltipSegment(TooltipSegmentType.Text, templatedTooltipSegment.Value));
    }

    private List<TooltipSegment> GetNumericTooltipSegments(
        int baseNumericValue,
        int bonusNumericValue
    )
    {
        List<TooltipSegment> tooltipSegments = ListUtils.Singleton(
            new TooltipSegment(TooltipSegmentType.BaseNumericValue, baseNumericValue.ToString())
        );

        if (bonusNumericValue > 0) tooltipSegments.Add(new TooltipSegment(TooltipSegmentType.BonusNumericValue, bonusNumericValue.ToString()));

        return tooltipSegments;
    }
}
