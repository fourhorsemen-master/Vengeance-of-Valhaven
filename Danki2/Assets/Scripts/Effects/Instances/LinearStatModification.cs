public class LinearStatModification : Effect
{
    private readonly Stat statToModify;
    private readonly int modification;

    public LinearStatModification(Stat statToModify, int modification)
    {
        this.statToModify = statToModify;
        this.modification = modification;
    }

    public override int GetLinearStatModifier(Stat stat)
    {
        return stat == statToModify ? modification : 0;
    }
}
