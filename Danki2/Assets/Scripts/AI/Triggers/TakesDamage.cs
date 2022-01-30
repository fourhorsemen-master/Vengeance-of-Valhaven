public class TakesDamage : SubjectTrigger
{
    public TakesDamage(Actor actor)
        : base(actor.HealthManager.DamageSubject)
    {
    }
}
