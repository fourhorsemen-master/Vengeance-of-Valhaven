using UnityEngine;

public class ChannelScreenBar : ScreenBar
{
    [SerializeField]
    private Player _player = null;

    private void Awake()
    {
        SetWidth(0f);
    }

    private void Update()
    {
        if (
            _player.CastingStatus != CastingStatus.ChannelingLeft
            && _player.CastingStatus != CastingStatus.ChannelingRight
        )
        {
            SetWidth(0f);
            return;
        }

        SetWidth(_player.RemainingChannelDuration / _player.TotalChannelDuration);
    }
}
