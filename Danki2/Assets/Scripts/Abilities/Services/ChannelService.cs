using UnityEngine;

public class ChannelService
{
    private Channel _currentChannel;
    public float RemainingDuration { get; private set; }
    public float TotalDuration => _currentChannel.Duration;

    public ChannelService()
    {
        Active = false;
    }

    public bool Active { get; private set; }

    public void Update()
    {
        if (!Active)
        {
            RemainingDuration = 0f;
            return;
        };

        this.RemainingDuration = Mathf.Max(0f, RemainingDuration - Time.deltaTime);

        if (RemainingDuration > 0f)
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
        RemainingDuration = _currentChannel.Duration;
        Active = true;
        _currentChannel.Start();
    }

    public void Cancel()
    {
        if (!Active) return;

        _currentChannel.Cancel();
        RemainingDuration = 0f;
        Active = false;
    }
}