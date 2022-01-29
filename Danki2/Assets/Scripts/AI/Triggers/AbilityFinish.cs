public class AbilityFinish : SubjectTrigger
{
    public AbilityFinish(Actor actor)
        : base(actor.AbilityAnimationListener.FinishSubject)
    {
    }
}
