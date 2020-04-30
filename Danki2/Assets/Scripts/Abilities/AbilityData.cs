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

    public static AbilityData operator +(AbilityData a, AbilityData b)
    {
        return new AbilityData(a.Damage + b.Damage, a.Heal + b.Heal, a.Shield + b.Shield);
    }

    public static AbilityData FromOrbCount(OrbType abilityOrbType, int orbCount)
    {
        AbilityData abilityData = Zero;

        switch (abilityOrbType)
        {
            case OrbType.Aggression:
                abilityData += new AbilityData(orbCount, 0, 0);
                break;
            case OrbType.Balance:
                abilityData += new AbilityData(0, orbCount, 0);
                break;
            case OrbType.Cunning:
                abilityData += new AbilityData(0, 0, orbCount);
                break;
        }

        return abilityData;
    }
}
