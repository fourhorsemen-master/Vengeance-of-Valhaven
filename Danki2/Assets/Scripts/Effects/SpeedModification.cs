public class SpeedModification : Effect
{
    private readonly int speedModification;

    public SpeedModification(int speedModification)
    {
        this.speedModification = speedModification;
    }

    public override float ProcessStat(Stat stat, float value)
    {
        return stat == Stat.Speed ? value + speedModification : value;
    }
}
