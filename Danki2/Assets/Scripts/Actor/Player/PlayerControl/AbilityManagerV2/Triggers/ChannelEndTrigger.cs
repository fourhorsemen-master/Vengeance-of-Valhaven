public class ChannelEndTrigger : SubjectTrigger
{
    public ChannelEndTrigger(Player player)
        : base(player.ChannelService.ChannelEndSubject)
    {
    }
}