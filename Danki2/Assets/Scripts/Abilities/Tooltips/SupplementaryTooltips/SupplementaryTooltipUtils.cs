using System.Collections.Generic;
using System.Linq;

public static class SupplementaryTooltipUtils
{
    private static readonly Dictionary<ChannelType, SupplementaryTooltip> channelTypeLookup =
        new Dictionary<ChannelType, SupplementaryTooltip>
        {
            {ChannelType.Channel, SupplementaryTooltip.Channel},
            {ChannelType.Cast, SupplementaryTooltip.Cast},
            {ChannelType.Charge, SupplementaryTooltip.Charge}
        };

    private static readonly Dictionary<TemplatedTooltipSegmentType, SupplementaryTooltip> effectLookup =
        new Dictionary<TemplatedTooltipSegmentType, SupplementaryTooltip>
        {
            {TemplatedTooltipSegmentType.Heal, SupplementaryTooltip.Heal},
            {TemplatedTooltipSegmentType.Shield, SupplementaryTooltip.Shield},
            {TemplatedTooltipSegmentType.Stun, SupplementaryTooltip.Stun},
            {TemplatedTooltipSegmentType.Slow, SupplementaryTooltip.Slow},
            {TemplatedTooltipSegmentType.Block, SupplementaryTooltip.Block},
            {TemplatedTooltipSegmentType.Bleed, SupplementaryTooltip.Bleed},
            {TemplatedTooltipSegmentType.Poison, SupplementaryTooltip.Poison},
            {TemplatedTooltipSegmentType.Vulnerable, SupplementaryTooltip.Vulnerable}
        };

    public static List<SupplementaryTooltip> GetSupplementaryTooltips(AbilityReference abilityReference)
    {
        List<SupplementaryTooltip> supplementaryTooltips = new List<SupplementaryTooltip>();

        if (AbilityLookup.Instance.IsFinisher(abilityReference)) supplementaryTooltips.Add(SupplementaryTooltip.Finisher);

        supplementaryTooltips.Add(GetAbilityType(abilityReference));

        supplementaryTooltips.AddRange(
            GetFromTemplatedTooltipSegments(AbilityLookup.Instance.GetTemplatedTooltipSegments(abilityReference))
        );

        foreach (AbilityBonusData abilityBonusData in AbilityLookup.Instance.GetAbilityBonusDataLookup(abilityReference).Values)
        {
            supplementaryTooltips.AddRange(GetFromTemplatedTooltipSegments(abilityBonusData.TemplatedTooltipSegments));
        }

        return supplementaryTooltips.Distinct().ToList();
    }

    private static SupplementaryTooltip GetAbilityType(AbilityReference abilityReference)
    {
        return AbilityLookup.Instance.TryGetChannelType(abilityReference, out ChannelType channelType)
            ? channelTypeLookup[channelType]
            : SupplementaryTooltip.InstantCast;
    }
    
    private static List<SupplementaryTooltip> GetFromTemplatedTooltipSegments(List<TemplatedTooltipSegment> templatedTooltipSegments)
    {
        List<SupplementaryTooltip> supplementaryTooltips = new List<SupplementaryTooltip>();

        templatedTooltipSegments.ForEach(templatedTooltipSegment =>
        {
            if (effectLookup.TryGetValue(templatedTooltipSegment.Type, out SupplementaryTooltip supplementaryTooltip))
            {
                supplementaryTooltips.Add(supplementaryTooltip);
            }
        });

        return supplementaryTooltips;
    }
}
