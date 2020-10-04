public class RandomAbilityManager
{
    private readonly Player player;

    public RandomAbilityManager(Player player)
    {
        this.player = player;

        RoomManager.Instance.WaveStartSubject.Subscribe(OnWaveStart);
    }

    private void OnWaveStart(int wave)
    {
        if (wave == 1) return;
        
        player.AbilityTree.AddToInventory(AbilityReference.Slash);
    }
}
