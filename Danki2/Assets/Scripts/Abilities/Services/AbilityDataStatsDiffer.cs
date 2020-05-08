public class AbilityDataStatsDiffer : IAbilityDataDiffer
{
    private readonly Actor actor;
    
    public AbilityDataStatsDiffer(Actor actor)
    {
        this.actor = actor;
    }
    
    public AbilityData GetAbilityDataDiff(AbilityReference abilityReference)
    {
        return GetAbilityDataDiff();
    }

    public AbilityData GetAbilityDataDiff(Node node)
    {
        return GetAbilityDataDiff();
    }

    private AbilityData GetAbilityDataDiff()
    {
        return new AbilityData(
            actor.GetStat(Stat.Power),
            actor.GetStat(Stat.Power),
            actor.GetStat(Stat.Recovery),
            actor.GetStat(Stat.Defence)
        );
    }
}
