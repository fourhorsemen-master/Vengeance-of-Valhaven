public class DamageData
{
    public int Damage { get; }
    public Actor Source { get; }

    public DamageData(int damage, Actor source)
    {
        Damage = damage;
        Source = source;
    }
}
