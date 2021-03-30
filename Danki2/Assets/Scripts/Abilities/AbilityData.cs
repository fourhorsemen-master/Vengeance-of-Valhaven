using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AbilityData
{
    [SerializeField]
    private int primaryDamage;
    [SerializeField]
    private int secondaryDamage;
    [SerializeField]
    private int heal;
    [SerializeField]
    private int shield;

    public int PrimaryDamage { get => primaryDamage; private set => primaryDamage = value; }
    public int SecondaryDamage { get => secondaryDamage; private set => secondaryDamage = value; }
    public int Heal { get => heal; private set => heal = value; }
    public int Shield { get => shield; private set => shield = value; }

    public AbilityData(int primaryDamage, int secondaryDamage, int heal, int shield)
    {
        this.primaryDamage = primaryDamage;
        this.secondaryDamage = secondaryDamage;
        this.heal = heal;
        this.shield = shield;
    }

    public static AbilityData Zero { get; } = new AbilityData(0, 0, 0, 0);
    public static AbilityData One { get; } = new AbilityData(1, 1, 1, 1);

    public static AbilityData operator +(AbilityData a, AbilityData b)
    {
        return new AbilityData(
            a.primaryDamage + b.primaryDamage,
            a.secondaryDamage + b.secondaryDamage,
            a.heal + b.heal,
            a.shield + b.shield
        );
    }

    public static AbilityData operator *(int i, AbilityData a)
    {
        return a * i;
    }

    public static AbilityData operator *(AbilityData a, int i)
    {
        return new AbilityData(
            a.primaryDamage * i,
            a.secondaryDamage * i,
            a.heal * i,
            a.shield * i
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
