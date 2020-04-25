﻿using System.Collections.Generic;

public static class TemplatedTooltipSegmentTypeLookup
{
    private static readonly Dictionary<BindableProperty, TemplatedTooltipSegmentType> bindablePropertyToTemplatedTooltipSegmentType =
        new Dictionary<BindableProperty, TemplatedTooltipSegmentType>
        {
            {BindableProperty.Damage, TemplatedTooltipSegmentType.Damage},
            {BindableProperty.Heal, TemplatedTooltipSegmentType.Heal},
            {BindableProperty.Shield, TemplatedTooltipSegmentType.Shield}
        };

    public static TemplatedTooltipSegmentType FromBindableProperty(BindableProperty bindableProperty)
    {
        return bindablePropertyToTemplatedTooltipSegmentType[bindableProperty];
    }
}
