using System.Collections.Generic;
using System.Linq;

public static class KeywordUtils
{
    private static readonly Dictionary<ChannelType, Keyword> channelTypeLookup =
        new Dictionary<ChannelType, Keyword>
        {
            {ChannelType.Channel, Keyword.Channel},
            {ChannelType.Cast, Keyword.Cast},
            {ChannelType.Charge, Keyword.Charge}
        };

    private static readonly Dictionary<TemplatedTooltipSegmentType, Keyword> effectLookup =
        new Dictionary<TemplatedTooltipSegmentType, Keyword>
        {
            {TemplatedTooltipSegmentType.Heal, Keyword.Heal},
            {TemplatedTooltipSegmentType.Shield, Keyword.Shield},
            {TemplatedTooltipSegmentType.Stun, Keyword.Stun},
            {TemplatedTooltipSegmentType.Slow, Keyword.Slow},
            {TemplatedTooltipSegmentType.Block, Keyword.Block},
            {TemplatedTooltipSegmentType.Bleed, Keyword.Bleed},
            {TemplatedTooltipSegmentType.Poison, Keyword.Poison},
            {TemplatedTooltipSegmentType.Vulnerable, Keyword.Vulnerable}
        };

    public static List<Keyword> GetKeywords(AbilityReference abilityReference)
    {
        List<Keyword> keywords = new List<Keyword>();

        if (AbilityLookup.Instance.IsFinisher(abilityReference)) keywords.Add(Keyword.Finisher);

        keywords.Add(GetAbilityType(abilityReference));

        keywords.AddRange(
            GetFromTemplatedTooltipSegments(AbilityLookup.Instance.GetTemplatedTooltipSegments(abilityReference))
        );

        foreach (AbilityBonusData abilityBonusData in AbilityLookup.Instance.GetAbilityBonusDataLookup(abilityReference).Values)
        {
            keywords.AddRange(GetFromTemplatedTooltipSegments(abilityBonusData.TemplatedTooltipSegments));
        }

        return keywords.Distinct().ToList();
    }

    public static List<Keyword> GetKeywords(Rune rune)
    {
        List<Keyword> keywords = new List<Keyword>();

        keywords.AddRange(GetFromTemplatedTooltipSegments(RuneLookup.Instance.GetTemplatedTooltipSegments(rune)));

        return keywords.Distinct().ToList();
    }

    private static Keyword GetAbilityType(AbilityReference abilityReference)
    {
        return AbilityLookup.Instance.TryGetChannelType(abilityReference, out ChannelType channelType)
            ? channelTypeLookup[channelType]
            : Keyword.InstantCast;
    }
    
    private static List<Keyword> GetFromTemplatedTooltipSegments(List<TemplatedTooltipSegment> templatedTooltipSegments)
    {
        List<Keyword> Keywords = new List<Keyword>();

        templatedTooltipSegments.ForEach(templatedTooltipSegment =>
        {
            if (effectLookup.TryGetValue(templatedTooltipSegment.Type, out Keyword Keyword))
            {
                Keywords.Add(Keyword);
            }
        });

        return Keywords;
    }
}
