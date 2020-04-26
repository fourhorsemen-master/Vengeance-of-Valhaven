public class ChannelScreenBar : ScreenBar
{
    private Player player;
    private ChannelService channelService;

    private void Awake()
    {
        SetWidth(0f);
    }

    private void Start()
    {
        player = RoomManager.Instance.Player;
        channelService = player.ChannelService;
    }

    private void Update()
    {
        if (
            player.AbilityManager.CastingStatus != CastingStatus.ChannelingLeft
            && player.AbilityManager.CastingStatus != CastingStatus.ChannelingRight
        )
        {
            SetWidth(0f);
            return;
        }
                
        SetWidth(channelService.RemainingDuration / channelService.TotalDuration);
    }
}
