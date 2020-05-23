using UnityEngine;
using UnityEngine.UI;

public class ChannelScreenBar : MonoBehaviour
{
    [SerializeField]
    private Image channelBar = null;

    [SerializeField]
    private Color channelColor;

    [SerializeField]
    private Color chargeColor;

    [SerializeField]
    private Color castColor;

    private Player player;
    private ChannelService channelService;

    private void Start()
    {
        player = RoomManager.Instance.Player;
        channelService = player.ChannelService;
        channelService.ChannelStartSubject.Subscribe(channelType => SetDisplay(channelType));
    }

    private void Update()
    {
        float width = 0f;

        if (
            player.AbilityManager.CastingStatus == CastingStatus.ChannelingLeft
            || player.AbilityManager.CastingStatus == CastingStatus.ChannelingRight
        )
        {
            width = channelService.RemainingDuration / channelService.TotalDuration;
        }

        channelBar.transform.localScale = new Vector3(width, 1f, 1f);
    }

    private void SetDisplay(ChannelType channelType)
    {
        switch (channelType)
        {
            case ChannelType.Channel:
                channelBar.color = channelColor;
                break;

            case ChannelType.Charge:
                channelBar.color = chargeColor;
                break;

            case ChannelType.Cast:
                channelBar.color = castColor;
                break;
        }
    }
}
