public class StatModification : Effect
{
    private readonly Stat statToModify;
    private readonly int modificationAmount;

    public StatModification(Stat statToModify, int modificationAmount)
    {
        this.statToModify = statToModify;
        this.modificationAmount = modificationAmount;
    }

    public override float ProcessStat(Stat stat, float value)
    {
        return stat == statToModify ? value + modificationAmount : value;
    }
}
