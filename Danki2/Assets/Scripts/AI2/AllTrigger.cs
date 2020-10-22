using System.Linq;

public class AllTrigger : IAiTrigger
{
    private readonly IAiTrigger[] triggers;
    
    public AllTrigger(params IAiTrigger[] triggers)
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
