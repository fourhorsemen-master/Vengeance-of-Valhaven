using UnityEngine;

public class ChannelService : AbilityService
{
    private Channel currentChannel;

    public bool Active { get; private set; }
    public float RemainingDuration { get; private set; }
    public float TotalDuration => currentChannel.Duration;

    public ChannelService(Actor owner) : base(owner)
    {
        Active = false;
    }

    public void Update(Vector3 targetPosition)
    {
        if (!Active)
        {
            RemainingDuration = 0f;
            return;
        };

        this.RemainingDuration = Mathf.Max(0f, RemainingDuration - Time.deltaTime);

        if (RemainingDuration > 0f)
        {
            currentChannel.Continue(GetAbilityContext(targetPosition));
        }
        else
        {
            currentChannel.End(GetAbilityContext(targetPosition));
            Active = false;
        }
    }

    public void Start(Channel channel, Vector3 targetPosition)
    {
        currentChannel = channel;
        RemainingDuration = currentChannel.Duration;
        Active = true;
        AbilityContext abilityContext = GetAbilityContext(targetPosition);
        currentChannel.Start(abilityContext);
        currentChannel.Continue(abilityContext);
    }

    public void Cancel(Vector3 targetPosition)
    {
        if (!Active) return;

        currentChannel.Cancel(GetAbilityContext(targetPosition));
        RemainingDuration = 0f;
        Active = false;
    }
}