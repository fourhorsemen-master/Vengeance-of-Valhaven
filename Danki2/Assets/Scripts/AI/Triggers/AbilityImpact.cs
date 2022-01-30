public class AbilityImpact : SubjectTrigger
{
    public AbilityImpact(Actor actor)
        : base(actor.AbilityAnimationListener.ImpactSubject)
    {
    }
}
