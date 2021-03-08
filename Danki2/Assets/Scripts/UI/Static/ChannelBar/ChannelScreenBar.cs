using UnityEngine;
using UnityEngine.UI;

public class ChannelScreenBar : MonoBehaviour
{
    public ChannelBarColourDictionary barColourLookup = new ChannelBarColourDictionary(Color.white);

    public Image channelBar = null;

    private Player player;
    private ChannelService channelService;

    private void Start()
    {
        player = ActorCache.Instance.Player;
        channelService = player.ChannelService;
        channelService.ChannelStartSubject.Subscribe(channelType =>
            channelBar.color = barColourLookup[channelType]
        );
    }

    private void Update()
    {
        float width = 0f;

        if (channelService.Active)
        {
            width = channelService.RemainingDuration / channelService.TotalDuration;
        }

        channelBar.transform.localScale = new Vector3(width, 1f, 1f);
    }
}
