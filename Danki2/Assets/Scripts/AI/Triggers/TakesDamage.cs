public class TakesDamage : IAiTrigger
{
    private readonly Actor actor;

    private bool takenDamage;
    private Subscription damageSubscription;

    public TakesDamage(Actor actor)
    {
        this.actor = actor;
    }

    public void Activate()
    {
        takenDamage = false;

        damageSubscription = actor.HealthManager.DamageSubject
            .Subscribe(() => takenDamage = true);
    }

    public void Deactivate()
    {
        damageSubscription.Unsubscribe();
    }

    public bool Triggers()
    {
        return takenDamage;
    }
}
