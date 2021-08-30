using System;
using System.Collections.Generic;
using System.Linq;

public static class PlayerListTooltipBuilder
{
    public static List<TooltipSegment> Build(SerializableGuid abilityId)
    {
        // List<TemplatedTooltipSegment> templatedTooltipSegments = AbilityLookup.Instance.GetTemplatedTooltipSegments(ability);
        //
        // List<TooltipSegment> tooltipSegments = TooltipUtils.GetTooltipPrefix(ability);
        // tooltipSegments.AddRange(GetTooltipSegments(templatedTooltipSegments, ability));
        //
        // return tooltipSegments;
        return ListUtils.Singleton(new TooltipSegment(TooltipSegmentType.Text, "This is a tooltip"));
    }

    public static List<TooltipSegment> BuildBonus(SerializableGuid abilityId, string bonus)
    {
        // List<TemplatedTooltipSegment> templatedTooltipSegments = AbilityLookup.Instance
        //     .GetAbilityBonusDataLookup(ability)[bonus]
        //     .TemplatedTooltipSegments;
        //
        // return GetTooltipSegments(templatedTooltipSegments, ability);
        return ListUtils.Singleton(new TooltipSegment(TooltipSegmentType.Text, "This is a bonus tooltip"));
    }

    // private static List<TooltipSegment> GetTooltipSegments(List<TemplatedTooltipSegment> templatedTooltipSegments, AbilityReference ability)
    // {
    //     return templatedTooltipSegments
    //         .Select(templatedTooltipSegment => GetTooltipSegment(
    //             templatedTooltipSegment,
    //             AbilityLookup.Instance.GetBaseAbilityData(ability)
    //         ))
    //         .ToList();
    // }
    //
    // private static TooltipSegment GetTooltipSegment(
    //     TemplatedTooltipSegment templatedTooltipSegment,
    //     AbilityData baseAbilityData
    // )
    // {
    //     if (TooltipBuilderUtils.CanHandle(templatedTooltipSegment))
    //     {
    //         return TooltipBuilderUtils.GetTooltipSegment(templatedTooltipSegment);
    //     }
    //
    //     switch (templatedTooltipSegment.Type)
    //     {
    //         case TemplatedTooltipSegmentType.PrimaryDamage:
    //             return GetNumericSegment(baseAbilityData.PrimaryDamage);
    //
    //         case TemplatedTooltipSegmentType.SecondaryDamage:
    //             return GetNumericSegment(baseAbilityData.SecondaryDamage);
    //
    //         case TemplatedTooltipSegmentType.Heal:
    //             return GetNumericSegment(baseAbilityData.Heal);
    //
    //         case TemplatedTooltipSegmentType.Shield:
    //             return GetNumericSegment(baseAbilityData.Shield);
    //
    //         default:
    //             throw new ArgumentOutOfRangeException();
    //     }
    // }
    //
    // private static TooltipSegment GetNumericSegment(int baseNumericValue)
    // {
    //     return new TooltipSegment(TooltipSegmentType.UnaffectedNumericValue, baseNumericValue.ToString());
    // }
}
