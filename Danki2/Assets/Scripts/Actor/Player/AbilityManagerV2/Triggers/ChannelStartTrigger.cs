public class ChannelStartTrigger : SubjectTrigger<ChannelType>
{
    public ChannelStartTrigger(Player player, Direction expectedDirection)
        : base(player.ChannelService.ChannelStartSubject, _ => player.LastCastDirection == expectedDirection)
    {
    }
}