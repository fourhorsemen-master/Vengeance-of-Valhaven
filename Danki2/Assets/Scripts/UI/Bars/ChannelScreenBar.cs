public class ChannelScreenBar : ScreenBar
{
    Player _player;

    void Awake()
    {
        SetWidth(0f);
        _player = FindObjectOfType<Player>();
    }

    void Update()
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
