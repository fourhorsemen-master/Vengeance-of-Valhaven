public class AbilityData
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
}
