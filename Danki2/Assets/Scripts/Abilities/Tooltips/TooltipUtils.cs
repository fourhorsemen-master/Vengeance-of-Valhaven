using System.Collections.Generic;

public static class TooltipUtils
{
    public static List<TooltipSegment> GetTooltipPrefix(AbilityReference abilityReference)
    {
        AbilityType abilityType = AbilityLookup.Instance.GetAbilityType(abilityReference);

        switch (abilityType)
        {
            case AbilityType.InstantCast:
                return ListUtils.Singleton(new TooltipSegment(TooltipSegmentType.Text, "Instant Cast - "));
            case AbilityType.Channel:
                return GetChannelPrefix(abilityReference);
        }

        return new List<TooltipSegment>();
    }

    private static List<TooltipSegment> GetChannelPrefix(AbilityReference abilityReference)
    {
        if (!AbilityLookup.Instance.TryGetChannelType(abilityReference, out ChannelType channelType)) return new List<TooltipSegment>();
        if (!AbilityLookup.Instance.TryGetChannelDuration(abilityReference, out float channelDuration)) return new List<TooltipSegment>();

        switch (channelType)
        {
            case ChannelType.Channel:
                return ListUtils.Singleton(new TooltipSegment(TooltipSegmentType.Text, $"Channel ({channelDuration}s) - "));
            case ChannelType.Charge:
                return ListUtils.Singleton(new TooltipSegment(TooltipSegmentType.Text, $"Charge ({channelDuration}s) - "));
            case ChannelType.Cast:
                return ListUtils.Singleton(new TooltipSegment(TooltipSegmentType.Text, $"Cast ({channelDuration}s) - "));
        }
        
        return new List<TooltipSegment>();
    }
}
