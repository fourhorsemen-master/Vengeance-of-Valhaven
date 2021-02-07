/// <summary>
/// Represents the type of values that out tooltip can be split up into.
/// </summary>
public enum TemplatedTooltipSegmentType
{
    Text,
    PrimaryDamage,
    SecondaryDamage,
    Heal,
    Shield,
    Stun,
    ActiveSlow,
    Slow,
    Block,
    Bleed,
    Poison,
    Vulnerable,
    // New templated tooltip segment types need a new entry adding in the templated tooltip segment type lookup
}
