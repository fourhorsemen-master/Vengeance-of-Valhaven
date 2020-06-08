public class LinearStatModification : Effect
{
    private readonly Stat statToModify;
    private readonly int modification;

    public LinearStatModification(Stat statToModify, int modification)
    {
        this.statToModify = statToModify;
        this.modification = modification;
    }

    public override float ProcessStat(Stat stat, float value)
    {
        return stat == statToModify ? value + modification : value;
    }
}
