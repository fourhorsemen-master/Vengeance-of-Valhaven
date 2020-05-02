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

    public List<TooltipSegment> Build(Node node)
    {
        AbilityReference abilityReference = node.Ability;
        List<TemplatedTooltipSegment> templatedTooltipSegments =
            ParsedTooltipLookup.GetTemplatedTooltipSegments(abilityReference);

        return templatedTooltipSegments
            .SelectMany(templatedTooltipSegment => GetTooltipSegments(
                templatedTooltipSegment,
                AbilityLookup.GetBaseAbilityData(abilityReference),
                GetAbilityDataDiff(node)
            ))
            .ToList();
    }

    private AbilityData GetAbilityDataDiff(Node node)
    {
        AbilityData abilityData = AbilityData.Zero;
        differs.ForEach(d => abilityData += d.GetAbilityDataDiff(node));
        return abilityData;
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
            case TemplatedTooltipSegmentType.Damage:
                return GetDamageSegments(baseAbilityData, abilityDataDiff);
            case TemplatedTooltipSegmentType.Heal:
                return GetHealSegments(baseAbilityData, abilityDataDiff);
            case TemplatedTooltipSegmentType.Shield:
                return GetShieldSegments(baseAbilityData, abilityDataDiff);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private List<TooltipSegment> GetTextSegments(TemplatedTooltipSegment templatedTooltipSegment)
    {
        return ListUtils.Singleton(new TooltipSegment(TooltipSegmentType.Text, templatedTooltipSegment.Value));
    }

    private List<TooltipSegment> GetDamageSegments(AbilityData baseAbilityData, AbilityData abilityDataDiff)
    {
        return GetNumericTooltipSegments(
            TooltipSegmentType.Damage,
            baseAbilityData.Damage,
            TooltipSegmentType.DamageBonus,
            abilityDataDiff.Damage
        );
    }

    private List<TooltipSegment> GetHealSegments(AbilityData baseAbilityData, AbilityData abilityDataDiff)
    {
        return GetNumericTooltipSegments(
            TooltipSegmentType.Heal,
            baseAbilityData.Heal,
            TooltipSegmentType.HealBonus,
            abilityDataDiff.Heal
        );
    }

    private List<TooltipSegment> GetShieldSegments(AbilityData baseAbilityData, AbilityData abilityDataDiff)
    {
        return GetNumericTooltipSegments(
            TooltipSegmentType.Shield,
            baseAbilityData.Shield,
            TooltipSegmentType.ShieldBonus,
            abilityDataDiff.Shield
        );
    }

    private List<TooltipSegment> GetNumericTooltipSegments(
        TooltipSegmentType baseTooltipSegmentType,
        int baseValue,
        TooltipSegmentType bonusTooltipSegmentType,
        int bonusValue
    )
    {
        List<TooltipSegment> tooltipSegments = ListUtils.Singleton(
            new TooltipSegment(baseTooltipSegmentType, baseValue.ToString())
        );

        if (bonusValue > 0) tooltipSegments.Add(new TooltipSegment(bonusTooltipSegmentType, bonusValue.ToString()));

        return tooltipSegments;
    }
}
