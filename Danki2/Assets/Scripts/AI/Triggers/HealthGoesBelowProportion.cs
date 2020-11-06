public class HealthGoesBelowProportion : AiTrigger
{
    private readonly Actor actor;
    private readonly float proportion;
    
    private bool canTrigger;

    public HealthGoesBelowProportion(Actor actor, float proportion)
    {
        this.actor = actor;
        this.proportion = proportion;
    }

    public override void Activate()
    {
        canTrigger = (float) actor.HealthManager.Health / actor.HealthManager.MaxHealth >= proportion;
    }

    public override void Deactivate() {}

    public override bool Triggers()
    {
        return canTrigger && (float) actor.HealthManager.Health / actor.HealthManager.MaxHealth < proportion;
    }
}
