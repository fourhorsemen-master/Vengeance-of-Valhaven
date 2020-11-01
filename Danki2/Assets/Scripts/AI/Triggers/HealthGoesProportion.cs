public class HealthGoesBelowProportion : IAiTrigger
{
    private readonly Actor actor;
    private readonly float proportion;
    
    private bool canTrigger;

    public HealthGoesBelowProportion(Actor actor, float proportion)
    {
        this.actor = actor;
        this.proportion = proportion;
    }

    public void Activate()
    {
        canTrigger = (float) actor.HealthManager.Health / actor.HealthManager.MaxHealth >= proportion;
    }

    public void Deactivate() {}

    public bool Triggers()
    {
        return canTrigger && (float) actor.HealthManager.Health / actor.HealthManager.MaxHealth < proportion;
    }
}
