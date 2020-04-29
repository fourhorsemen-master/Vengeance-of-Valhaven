public class HealthScreenBar : ScreenBar
{
    private Player player;

    private void Start()
    {
        player = RoomManager.Instance.Player;
    }

    private void Update()
    {
        SetWidth((float)player.HealthManager.Health/player.HealthManager.MaxHealth);
    }
}
