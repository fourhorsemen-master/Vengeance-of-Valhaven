using System.Linq;

public class IfAllTrigger : IAiTrigger
{
    private readonly IAiTrigger[] triggers;
    
    public IfAllTrigger(params IAiTrigger[] triggers)
    {
        this.triggers = triggers;
    }

    public void Activate()
    {
        foreach (IAiTrigger trigger in triggers)
        {
            trigger.Activate();
        }
    }

    public bool Triggers()
    {
        return triggers.All(t => t.Triggers());
    }

    public void Deactivate()
    {
        foreach (IAiTrigger trigger in triggers)
        {
            trigger.Deactivate();
        }
    }
}
