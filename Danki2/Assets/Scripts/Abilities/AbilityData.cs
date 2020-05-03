using System;
using System.Collections.Generic;

public struct AbilityData
{
    public int Damage { get; }
    public int Heal { get; }
    public int Shield { get; }

    public AbilityData(int damage, int heal, int shield)
    {
        Damage = damage;
        Heal = heal;
        Shield = shield;
    }

    public static AbilityData Zero { get; } = new AbilityData(0, 0, 0);
    public static AbilityData One { get; } = new AbilityData(1, 1, 1);

    public static AbilityData operator +(AbilityData a, AbilityData b)
    {
        return new AbilityData(
            a.Damage + b.Damage,
            a.Heal + b.Heal,
            a.Shield + b.Shield
        );
    }

    public static AbilityData operator *(int i, AbilityData a)
    {
        return a * i;
    }

    public static AbilityData operator *(AbilityData a, int i)
    {
        return new AbilityData(
            a.Damage * i,
            a.Heal * i,
            a.Shield * i
        );
    }

    public static AbilityData FromAbilityDataDiffers(List<IAbilityDataDiffer> differs, Node node)
    {
        return FromAbilityDataDiffers(differs, differ => differ.GetAbilityDataDiff(node));
    }

    public static AbilityData FromAbilityDataDiffers(List<IAbilityDataDiffer> differs, AbilityReference abilityReference)
    {
        return FromAbilityDataDiffers(differs, differ => differ.GetAbilityDataDiff(abilityReference));
    }

    private static AbilityData FromAbilityDataDiffers(
        List<IAbilityDataDiffer> differs,
        Func<IAbilityDataDiffer, AbilityData> getAbilityDataDiff
    )
    {
        AbilityData abilityData = Zero;
        differs.ForEach(d => abilityData += getAbilityDataDiff(d));
        return abilityData;
    }
}
