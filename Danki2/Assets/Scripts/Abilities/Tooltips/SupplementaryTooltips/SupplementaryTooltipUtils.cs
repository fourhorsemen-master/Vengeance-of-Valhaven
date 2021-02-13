using System.Collections.Generic;

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

    public static ISet<SupplementaryTooltip> GetSupplementaryTooltips(AbilityReference abilityReference)
    {
        ISet<SupplementaryTooltip> supplementaryTooltips = new HashSet<SupplementaryTooltip>();

        // Finisher
        if (AbilityLookup.Instance.IsFinisher(abilityReference)) supplementaryTooltips.Add(SupplementaryTooltip.Finisher);

        // Ability type
        supplementaryTooltips.Add(
            AbilityLookup.Instance.TryGetChannelType(abilityReference, out ChannelType channelType)
                ? channelTypeLookup[channelType]
                : SupplementaryTooltip.InstantCast);

        // Effects
        AbilityLookup.Instance
            .GetTemplatedTooltipSegments(abilityReference)
            .ForEach(templatedTooltipSegment =>
            {
                if (effectLookup.TryGetValue(templatedTooltipSegment.Type, out SupplementaryTooltip supplementaryTooltip))
                {
                    supplementaryTooltips.Add(supplementaryTooltip);
                }
            });

        return supplementaryTooltips;
    }
}
