public class IronSkinHandler : IStatPipe
{
    public float ProcessStat(Stat stat, float value)
    {
        if (stat == Stat.MaxHealth) return value * 2f;

        return value;
    }
}
