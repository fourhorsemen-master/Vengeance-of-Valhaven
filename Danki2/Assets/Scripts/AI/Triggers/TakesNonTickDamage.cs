public class TakesNonTickDamage : StateMachineTrigger
{
    private readonly Actor actor;

    private bool takenDamage;
    private Subscription<DamageData> damageSubscription;

    public TakesNonTickDamage(Actor actor)
    {
        this.actor = actor;
    }

    public override void Activate()
    {
        takenDamage = false;

        damageSubscription = actor.HealthManager.ModifiedDamageSubject
            .Subscribe(_ => takenDamage = true);
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
