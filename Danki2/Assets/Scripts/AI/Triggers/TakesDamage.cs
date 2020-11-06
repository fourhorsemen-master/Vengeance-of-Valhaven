public class TakesDamage : AiTrigger
{
    private readonly Actor actor;

    private bool takenDamage;
    private Subscription damageSubscription;

    public TakesDamage(Actor actor)
    {
        this.actor = actor;
    }

    public override void Activate()
    {
        takenDamage = false;

        damageSubscription = actor.HealthManager.DamageSubject
            .Subscribe(() => takenDamage = true);
    }

    public override void Deactivate()
    {
        damageSubscription.Unsubscribe();
    }

    public override bool Triggers()
    {
        return takenDamage;
    }
}
