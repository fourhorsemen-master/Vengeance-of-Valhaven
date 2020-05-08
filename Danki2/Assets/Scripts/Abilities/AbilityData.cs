﻿using System;
using System.Collections.Generic;

public struct AbilityData
{
    public int PrimaryDamage { get; }
    public int SecondaryDamage { get; }
    public int Heal { get; }
    public int Shield { get; }

    public AbilityData(int primaryDamage, int secondaryDamage, int heal, int shield)
    {
        PrimaryDamage = primaryDamage;
        SecondaryDamage = secondaryDamage;
        Heal = heal;
        Shield = shield;
    }

    public static AbilityData Zero { get; } = new AbilityData(0, 0, 0, 0);
    public static AbilityData One { get; } = new AbilityData(1, 1, 1, 1);

    public static AbilityData operator +(AbilityData a, AbilityData b)
    {
        return new AbilityData(
            a.PrimaryDamage + b.PrimaryDamage,
            a.SecondaryDamage + b.SecondaryDamage,
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
            a.PrimaryDamage * i,
            a.SecondaryDamage * i,
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
