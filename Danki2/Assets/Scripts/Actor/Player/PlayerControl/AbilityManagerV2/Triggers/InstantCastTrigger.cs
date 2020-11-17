public class InstantCastTrigger : SubjectTrigger
{
    public InstantCastTrigger(Player player)
        : base(player.InstantCastService.CastSubject)
    {
    }
}
