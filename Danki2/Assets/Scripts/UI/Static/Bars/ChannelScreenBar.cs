using UnityEngine;
using UnityEngine.UI;

public class ChannelScreenBar : MonoBehaviour
{
    [SerializeField]
    private Image channelBar;

    private Player player;
    private ChannelService channelService;
    
    private void Start()
    {
        player = RoomManager.Instance.Player;
        channelService = player.ChannelService;
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
}
