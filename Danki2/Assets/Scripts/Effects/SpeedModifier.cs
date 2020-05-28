public class SpeedModifier : Effect
{
    private readonly int speedModification;

    public SpeedModifier(int speedModification)
    {
        this.speedModification = speedModification;
    }

    public override float ProcessStat(Stat stat, float value)
    {
        return stat == Stat.Speed ? value + speedModification : value;
    }
}
