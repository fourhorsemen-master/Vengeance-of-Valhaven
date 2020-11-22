public class AbilityTreeUpdatedTrigger : SubjectTrigger
{
    public AbilityTreeUpdatedTrigger(Player player) : base(player.AbilityTree.ResettingChangeSubject)
    {
    }
}