public class CooldownScreenBar : ScreenBar
{
    private Player player;

    private void Awake()
    {
        SetWidth(0f);
    }

    private void Start()
    {
        player = RoomManager.Instance.Player;
    }

    private void Update()
    {
        //SetWidth(player.AbilityManager.remainingAbilityCooldown / player.abilityCooldown);
    }
}
