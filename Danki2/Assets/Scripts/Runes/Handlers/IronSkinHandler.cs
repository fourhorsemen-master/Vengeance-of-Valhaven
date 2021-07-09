public class IronSkinHandler : IStatPipe
{
    private const int HealthBonus = 10;

    private readonly RuneManager runeManager;
    
    public IronSkinHandler(RuneManager runeManager, Player player)
    {
        this.runeManager = runeManager;

        runeManager.RuneAddedSubject
            .Where(rune => rune == Rune.IronSkin)
            .Subscribe(_ =>
            {
                player.StatsManager.ClearCache();
                player.HealthManager.ReceiveHeal(HealthBonus);
            });

        runeManager.RuneRemovedSubject
            .Where(rune => rune == Rune.IronSkin)
            .Subscribe(_ => player.StatsManager.ClearCache());
    }

    public float ProcessStat(Stat stat, float value)
    {
        return stat == Stat.MaxHealth && runeManager.HasRune(Rune.IronSkin)
            ? value + HealthBonus
            : value;
    }
}
