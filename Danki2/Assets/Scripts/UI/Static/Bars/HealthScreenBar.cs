using UnityEngine;

public class HealthScreenBar : ScreenBar
{
    private Player player;

    private void Start()
    {
        this.player = RoomManager.Instance.Player;
    }

    private void Update()
    {
        SetWidth(Mathf.Max(this.player.Health, 0f) / this.player.GetStat(Stat.MaxHealth));
    }
}
