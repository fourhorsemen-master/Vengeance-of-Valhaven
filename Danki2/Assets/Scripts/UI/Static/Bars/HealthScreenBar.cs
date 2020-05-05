public class HealthScreenBar : HealthBar
{
    protected override Actor Actor => RoomManager.Instance.Player;
}
