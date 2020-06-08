public class MultiplicativeStatModification : Effect
{
    private readonly Stat statToModify;
    private readonly float modification;

    public MultiplicativeStatModification(Stat statToModify, float modification)
    {
        this.statToModify = statToModify;
        this.modification = modification;
    }

    public override float ProcessStat(Stat stat, float value)
    {
        return stat == statToModify ? value * modification : value;
    }
}
