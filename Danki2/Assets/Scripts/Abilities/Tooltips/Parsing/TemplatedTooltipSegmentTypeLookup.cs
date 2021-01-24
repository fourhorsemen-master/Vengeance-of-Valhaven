using System.Collections.Generic;

public static class TemplatedTooltipSegmentTypeLookup
{
    private static readonly Dictionary<BindableProperty, TemplatedTooltipSegmentType> bindablePropertyToTemplatedTooltipSegmentType =
        new Dictionary<BindableProperty, TemplatedTooltipSegmentType>
        {
            {BindableProperty.PrimaryDamage, TemplatedTooltipSegmentType.PrimaryDamage},
            {BindableProperty.SecondaryDamage, TemplatedTooltipSegmentType.SecondaryDamage},
            {BindableProperty.Heal, TemplatedTooltipSegmentType.Heal},
            {BindableProperty.Shield, TemplatedTooltipSegmentType.Shield},
            {BindableProperty.Stun, TemplatedTooltipSegmentType.Stun},
            {BindableProperty.PassiveSlow, TemplatedTooltipSegmentType.PassiveSlow},
            {BindableProperty.Block, TemplatedTooltipSegmentType.Block},
            {BindableProperty.StackingSlow, TemplatedTooltipSegmentType.StackingSlow},
            {BindableProperty.Bleed, TemplatedTooltipSegmentType.Bleed},
            {BindableProperty.Poison, TemplatedTooltipSegmentType.Poison},
            {BindableProperty.Vulnerable, TemplatedTooltipSegmentType.Vulnerable},
        };

    public static TemplatedTooltipSegmentType FromBindableProperty(BindableProperty bindableProperty)
    {
        return bindablePropertyToTemplatedTooltipSegmentType[bindableProperty];
    }
}
