public class IronSkinHandler : IStatPipe
{
    private const int HealthBonus = 20;

    private readonly RuneManager runeManager;
    
    public IronSkinHandler(RuneManager runeManager, Player player)
    {
        this.runeManager = runeManager;

        runeManager.RuneAddedSubject
            .Subscribe(rune =>
            {
                if (rune != Rune.IronSkin) return;

                player.StatsManager.ClearCache();
                player.HealthManager.ReceiveHeal(HealthBonus);
            });
    }

    public float ProcessStat(Stat stat, float value)
    {
        return stat == Stat.MaxHealth && runeManager.HasRune(Rune.IronSkin)
            ? value + HealthBonus
            : value;
    }
}
