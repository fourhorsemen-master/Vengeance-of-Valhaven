using System.Linq;

public class AndTrigger : IAiTrigger
{
    private readonly IAiTrigger[] triggers;
    
    public AndTrigger(params IAiTrigger[] triggers)
    {
        this.triggers = triggers;
    }

    public void Activate()
    {
        foreach (IAiTrigger trigger in triggers) trigger.Activate();
    }

    public void Deactivate()
    {
        foreach (IAiTrigger trigger in triggers) trigger.Deactivate();
    }

    public bool Triggers()
    {
        return triggers.All(t => t.Triggers());
    }
}
