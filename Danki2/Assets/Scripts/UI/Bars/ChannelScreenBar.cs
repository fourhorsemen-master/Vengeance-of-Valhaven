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
        this.player = RoomManager.Instance.Player;
        this.channelService = this.player.ChannelService;
    }

    private void Update()
    {
        if (
            this.player.CastingStatus != CastingStatus.ChannelingLeft
            && this.player.CastingStatus != CastingStatus.ChannelingRight
        )
        {
            SetWidth(0f);
            return;
        }
                
        SetWidth(this.channelService.RemainingDuration / this.channelService.TotalDuration);
    }
}
