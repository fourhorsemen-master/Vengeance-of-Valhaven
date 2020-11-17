public class ComboFailedTrigger : SubjectTrigger
{
    public ComboFailedTrigger(Player player)
        : base(player.ComboFailedSubject)
    {
    }
}