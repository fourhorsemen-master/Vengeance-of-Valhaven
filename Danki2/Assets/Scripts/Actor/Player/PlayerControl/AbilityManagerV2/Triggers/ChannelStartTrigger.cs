public class ChannelStartTrigger : SubjectTrigger<Direction>
{
    public ChannelStartTrigger(Player player, Direction expectedDirection)
        : base(player.ChannelStartSubject, d => d == expectedDirection)
    {
    }
}