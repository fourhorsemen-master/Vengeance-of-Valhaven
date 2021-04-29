public class FleetOfFootHandler : IStatPipe
{
    private const int SpeedBonus = 2;

    private readonly RuneManager runeManager;
    
    public FleetOfFootHandler(RuneManager runeManager, Player player)
    {
        this.runeManager = runeManager;

        runeManager.RuneAddedSubject
            .Where(rune => rune == Rune.FleetOfFoot)
            .Subscribe(rune => player.StatsManager.ClearCache());
    }

    public float ProcessStat(Stat stat, float value)
    {
        return stat == Stat.Speed && runeManager.HasRune(Rune.FleetOfFoot)
            ? value + SpeedBonus
            : value;
    }
}
