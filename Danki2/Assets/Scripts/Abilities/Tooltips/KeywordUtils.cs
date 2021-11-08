using System.Collections.Generic;
using System.Linq;

public static class KeywordUtils
{
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
    
    private static readonly Dictionary<Empowerment, Keyword> empowermentLookup =
        new Dictionary<Empowerment, Keyword>
        {
            [Empowerment.Impact] = Keyword.Impact,
            [Empowerment.Rupture] = Keyword.Rupture,
            [Empowerment.Execute] = Keyword.Execute,
            [Empowerment.Maim] = Keyword.Maim,
            [Empowerment.Duel] = Keyword.Duel,
            [Empowerment.Brawl] = Keyword.Brawl,
            [Empowerment.Shock] = Keyword.Shock,
            [Empowerment.Siphon] = Keyword.Siphon,
        };

    public static List<Keyword> GetKeywords(SerializableGuid abilityId)
    {
        // List<Keyword> keywords = new List<Keyword>();
        //
        // keywords.Add(GetAbilityType(abilityReference));
        //
        // keywords.AddRange(
        //     GetFromTemplatedTooltipSegments(AbilityLookup.Instance.GetTemplatedTooltipSegments(abilityReference))
        // );
        //
        // foreach (AbilityBonusData abilityBonusData in AbilityLookup.Instance.GetAbilityBonusDataLookup(abilityReference).Values)
        // {
        //     keywords.AddRange(GetFromTemplatedTooltipSegments(abilityBonusData.TemplatedTooltipSegments));
        // }
        //
        // return keywords.Distinct().ToList();

        List<Keyword> keywords = new List<Keyword>();

        List<Empowerment> empowerments = AbilityLookup.Instance.GetEmpowerments(abilityId);
        empowerments.ForEach(empowerment => keywords.Add(empowermentLookup[empowerment]));

        return keywords;
    }

    public static List<Keyword> GetKeywords(Rune rune)
    {
        List<Keyword> keywords = new List<Keyword>();

        keywords.AddRange(GetFromTemplatedTooltipSegments(RuneLookup.Instance.GetTemplatedTooltipSegments(rune)));

        return keywords.Distinct().ToList();
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
