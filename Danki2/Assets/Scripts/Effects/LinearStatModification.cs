public class LinearStatModification : Effect
{
    private readonly Stat statToModify;
    private readonly int modification;

    public LinearStatModification(Stat statToModify, int modification)
    {
        this.statToModify = statToModify;
        this.modification = modification;
    }

    public override int GetAdditiveStatModifier(Stat stat)
    {
        return stat == statToModify ? modification : 0;
    }
}
