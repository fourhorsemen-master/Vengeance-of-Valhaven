using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerTreeTooltipBuilder
{
    private readonly List<IAbilityDataDiffer> differs = new List<IAbilityDataDiffer>();

    public PlayerTreeTooltipBuilder(Player player)
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
        List<TemplatedTooltipSegment> templatedTooltipSegments = AbilityLookup.Instance.GetTemplatedTooltipSegments(node.Ability);

        return GetTooltipSegments(templatedTooltipSegments, node);
    }

    /// <summary>
    /// Builds the tooltip for the specified bonus of a given node returning a list of tooltip segments.
    /// </summary
    /// <param name="node"> The node to build the tooltip for </param>
    /// <param name="bonus"> The bonus </param>
    /// <returns> The build tooltip segments </returns>
    public List<TooltipSegment> BuildBonus(Node node, string bonus)
    {
        List<TemplatedTooltipSegment> templatedTooltipSegments = AbilityLookup.Instance
            .GetAbilityBonusDataLookup(node.Ability)[bonus]
            .TemplatedTooltipSegments;

        return GetTooltipSegments(templatedTooltipSegments, node);
    }

    private List<TooltipSegment> GetTooltipSegments(List<TemplatedTooltipSegment> templatedTooltipSegments, Node node)
    {
        return templatedTooltipSegments
            .Select(templatedTooltipSegment => GetTooltipSegment(
                templatedTooltipSegment,
                AbilityLookup.Instance.GetBaseAbilityData(node.Ability),
                AbilityData.FromAbilityDataDiffers(differs, node)
            ))
            .ToList();
    }

    private TooltipSegment GetTooltipSegment(
        TemplatedTooltipSegment templatedTooltipSegment,
        AbilityData baseAbilityData,
        AbilityData abilityDataDiff
    )
    {
        switch (templatedTooltipSegment.Type)
        {
            case TemplatedTooltipSegmentType.Text:
                return GetTextSegment(templatedTooltipSegment);

            case TemplatedTooltipSegmentType.PrimaryDamage:
                return GetNumericTooltipSegment(baseAbilityData.PrimaryDamage, abilityDataDiff.PrimaryDamage);

            case TemplatedTooltipSegmentType.SecondaryDamage:
                return GetNumericTooltipSegment(baseAbilityData.SecondaryDamage, abilityDataDiff.SecondaryDamage);

            case TemplatedTooltipSegmentType.Heal:
                return GetNumericTooltipSegment(baseAbilityData.Heal, abilityDataDiff.Heal);

            case TemplatedTooltipSegmentType.Shield:
                return GetNumericTooltipSegment(baseAbilityData.Shield, abilityDataDiff.Shield);

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private TooltipSegment GetTextSegment(TemplatedTooltipSegment templatedTooltipSegment)
    {
        return new TooltipSegment(TooltipSegmentType.Text, templatedTooltipSegment.Value);
    }

    private TooltipSegment GetNumericTooltipSegment(
        int baseNumericValue,
        int bonusNumericValue
    )
    {
        TooltipSegmentType tooltipSegmentType = TooltipSegmentType.UnaffectedNumericValue;
        if (bonusNumericValue > 0) tooltipSegmentType = TooltipSegmentType.BuffedNumericValue;
        if (bonusNumericValue < 0) tooltipSegmentType = TooltipSegmentType.DebuffedNumericValue;

        return new TooltipSegment(
            tooltipSegmentType,
            (baseNumericValue + bonusNumericValue).ToString()
        );
    }
}
