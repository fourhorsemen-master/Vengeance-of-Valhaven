using UnityEngine;

public class ChannelService : MonoBehaviour
{
    private Channel _currentChannel;
    private float _remainingDuration;

    public ChannelService()
    {
        Active = false;
    }

    public bool Active { get; private set; }

    public void Update()
    {
        if (!Active)
        {
            _remainingDuration = 0f;
            return;
        };

        this._remainingDuration = Mathf.Min(0f, _remainingDuration - Time.deltaTime);

        if (_remainingDuration > 0)
        {
            _currentChannel.Continue();
        }
        else
        {
            _currentChannel.End();
            Active = false;
        }
    }

    public void Start(Channel channel)
    {
        _currentChannel = channel;
        _remainingDuration = _currentChannel.Duration;
        _currentChannel.Start();
    }

    public void Cancel()
    {
        if (!Active) return;

        _currentChannel.Cancel();
        Active = false;
    }
}