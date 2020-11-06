using System.Linq;

public abstract class AiTrigger
{
    public abstract void Activate();
    public abstract void Deactivate();
    public abstract bool Triggers();

    public static AiTrigger operator &(AiTrigger t1, AiTrigger t2)
    {
        return new AndTrigger(t1, t2);
    }

    public static AiTrigger operator |(AiTrigger t1, AiTrigger t2)
    {
        return new OrTrigger(t1, t2);
    }
    
    private class AndTrigger : AiTrigger
    {
        private readonly AiTrigger[] triggers;
    
        public AndTrigger(params AiTrigger[] triggers)
        {
            this.triggers = triggers;
        }

        public override void Activate()
        {
            foreach (AiTrigger trigger in triggers) trigger.Activate();
        }

        public override void Deactivate()
        {
            foreach (AiTrigger trigger in triggers) trigger.Deactivate();
        }

        public override bool Triggers()
        {
            return triggers.All(t => t.Triggers());
        }
    }
    
    private class OrTrigger : AiTrigger
    {
        private readonly AiTrigger[] triggers;
    
        public OrTrigger(params AiTrigger[] triggers)
        {
            this.triggers = triggers;
        }

        public override void Activate()
        {
            foreach (AiTrigger trigger in triggers) trigger.Activate();
        }

        public override void Deactivate()
        {
            foreach (AiTrigger trigger in triggers) trigger.Deactivate();
        }

        public override bool Triggers()
        {
            return triggers.Any(t => t.Triggers());
        }
    }
}
