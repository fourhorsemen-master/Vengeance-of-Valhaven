using System.Collections.Generic;

public static class TemplatedTooltipSegmentTypeLookup
{
    private static readonly Dictionary<BindableProperty, TemplatedTooltipSegmentType> bindablePropertyToTemplatedTooltipSegmentType =
        new Dictionary<BindableProperty, TemplatedTooltipSegmentType>
        {
            {BindableProperty.Heal, TemplatedTooltipSegmentType.Heal},
            {BindableProperty.Stun, TemplatedTooltipSegmentType.Stun},
            {BindableProperty.Slow, TemplatedTooltipSegmentType.Slow},
            {BindableProperty.Block, TemplatedTooltipSegmentType.Block},
            {BindableProperty.Bleed, TemplatedTooltipSegmentType.Bleed},
            {BindableProperty.Poison, TemplatedTooltipSegmentType.Poison},
            {BindableProperty.Vulnerable, TemplatedTooltipSegmentType.Vulnerable},
        };

    public static TemplatedTooltipSegmentType FromBindableProperty(BindableProperty bindableProperty)
    {
        return bindablePropertyToTemplatedTooltipSegmentType[bindableProperty];
    }
}
