public class TakesNonTickDamage : SubjectTrigger
{
    public TakesNonTickDamage(Actor actor)
        :base(actor.HealthManager.ModifiedDamageSubject.Flatten())
    {
    }
}
