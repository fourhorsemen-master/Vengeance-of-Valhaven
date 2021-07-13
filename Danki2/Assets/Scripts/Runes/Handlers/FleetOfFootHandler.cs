public class FleetOfFootHandler : IStatPipe
{
    private const float SpeedMultiplierBonus = 1.25f;

    private readonly RuneManager runeManager;
    
    public FleetOfFootHandler(RuneManager runeManager, Player player)
    {
        this.runeManager = runeManager;

        runeManager.RuneAddedSubject
            .Where(rune => rune == Rune.FleetOfFoot)
            .Subscribe(_ => player.StatsManager.ClearCache());
        
        runeManager.RuneRemovedSubject
            .Where(rune => rune == Rune.FleetOfFoot)
            .Subscribe(_ => player.StatsManager.ClearCache());
    }

    public float ProcessStat(Stat stat, float value)
    {
        return stat == Stat.Speed && runeManager.HasRune(Rune.FleetOfFoot)
            ? value * SpeedMultiplierBonus
            : value;
    }
}
