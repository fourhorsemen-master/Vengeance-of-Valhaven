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

    public static AbilityData FromGeneratedOrbs(EnumDictionary<OrbType, int> generatedOrbs)
    {
        return new AbilityData(
            generatedOrbs[OrbType.Aggression],
            generatedOrbs[OrbType.Balance],
            generatedOrbs[OrbType.Cunning]
        );
    }
}
