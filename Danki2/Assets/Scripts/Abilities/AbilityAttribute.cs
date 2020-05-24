using System;

/// <summary>
/// Attribute to be placed on all abilities. This is to explicitly mark to the ability reference that corresponds
/// to the ability class.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AbilityAttribute : Attribute
{
    public AbilityReference AbilityReference { get; }
    public string[] AbilityBonuses { get; }

    public AbilityAttribute(AbilityReference abilityReference, string[] abilityBonuses = null)
    {
        AbilityReference = abilityReference;
        AbilityBonuses = abilityBonuses ?? new string[0];
    }
}
