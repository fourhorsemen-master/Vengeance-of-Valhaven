public class HealthGoesBelow : IAiTrigger
{
    private readonly Actor actor;
    private readonly int threshold;
    
    private bool canTrigger;

    public HealthGoesBelow(Actor actor, int threshold)
    {
        this.actor = actor;
        this.threshold = threshold;
    }

    public void Activate()
    {
        canTrigger = actor.HealthManager.Health >= threshold;
    }

    public void Deactivate() {}

    public bool Triggers()
    {
        return canTrigger && actor.HealthManager.Health < threshold;
    }
}
