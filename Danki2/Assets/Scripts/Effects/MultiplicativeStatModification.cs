public class MultiplicativeStatModification : Effect
{
    private readonly Stat statToModify;
    private readonly float modification;

    public MultiplicativeStatModification(Stat statToModify, float modification)
    {
        this.statToModify = statToModify;
        this.modification = modification;
    }

    public override float GetMultiplicativeStatModifier(Stat stat)
    {
        return stat == statToModify ? modification : 1;
    }
}
