public class HealthGoesBelowProportion : AiTrigger
{
    private readonly Actor actor;
    private readonly float proportion;
    
    private bool canTrigger;

    private float HealthProportion => (float) actor.HealthManager.Health / actor.HealthManager.MaxHealth;

    public HealthGoesBelowProportion(Actor actor, float proportion)
    {
        this.actor = actor;
        this.proportion = proportion;
    }

    public override void Activate()
    {
        canTrigger = HealthProportion >= proportion;
    }

    public override void Deactivate() {}

    public override bool Triggers()
    {
        if (HealthProportion >= proportion)
        {
            canTrigger = true;
        }

        return canTrigger && HealthProportion < proportion;
    }
}
