using UnityEngine;

public class ChannelScreenBar : ScreenBar
{
    private Player player;

    private void Awake()
    {
        SetWidth(0f);
    }

    private void Start()
    {
        this.player = RoomManager.Instance.Player;
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

        ChannelService channelService = this.player.ChannelService;
        SetWidth(channelService.RemainingDuration / channelService.TotalDuration);
    }
}
