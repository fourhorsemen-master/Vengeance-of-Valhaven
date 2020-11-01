using System.Linq;

public class OrTrigger : IAiTrigger
{
    private readonly IAiTrigger[] triggers;
    
    public OrTrigger(params IAiTrigger[] triggers)
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
        return triggers.Any(t => t.Triggers());
    }
}
