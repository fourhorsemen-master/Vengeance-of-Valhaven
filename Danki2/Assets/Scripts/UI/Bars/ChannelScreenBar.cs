using UnityEngine;

public class ChannelScreenBar : ScreenBar
{
    [SerializeField]
    private Player player = null;

    private void Awake()
    {
        SetWidth(0f);
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
