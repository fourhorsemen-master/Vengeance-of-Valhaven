public class Slow : Effect
{
    private readonly float _slowMultiplier;

    public Slow(float slowMultiplier)
    {
        _slowMultiplier = slowMultiplier;
    }

    public override float ProcessStat(Stat stat, float value)
    {
        return stat == Stat.Speed
            ? value * _slowMultiplier
            : value;
    }
}
