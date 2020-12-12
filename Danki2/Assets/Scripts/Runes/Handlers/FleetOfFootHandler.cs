public class FleetOfFootHandler : IStatPipe
{
    public float ProcessStat(Stat stat, float value)
    {
        if (stat == Stat.Speed) return value * 1.3f;

        return value;
    }
}
