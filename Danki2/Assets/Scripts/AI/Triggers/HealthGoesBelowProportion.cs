public class HealthGoesBelowProportion : StateMachineTrigger
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
        canTrigger = actor.HealthManager.HealthProportion >= proportion;
    }

    public override void Deactivate() {}

    public override bool Triggers()
    {
        if (actor.HealthManager.HealthProportion >= proportion)
        {
            canTrigger = true;
        }

        return canTrigger && actor.HealthManager.HealthProportion < proportion;
    }
}
