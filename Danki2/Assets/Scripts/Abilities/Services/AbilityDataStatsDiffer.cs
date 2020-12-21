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
            actor.StatsManager.Get(Stat.Power),
            actor.StatsManager.Get(Stat.Power),
            actor.StatsManager.Get(Stat.Recovery),
            actor.StatsManager.Get(Stat.Defence)
        );
    }
}
