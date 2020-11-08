public abstract class AiTrigger
{
    public abstract void Activate();
    public abstract void Deactivate();
    public abstract bool Triggers();

    public static AiTrigger operator &(AiTrigger t1, AiTrigger t2) => new AndTrigger(t1, t2);
    public static AiTrigger operator |(AiTrigger t1, AiTrigger t2) => new OrTrigger(t1, t2);
    public static AiTrigger operator ^(AiTrigger t1, AiTrigger t2) => new XOrTrigger(t1, t2);
    public static AiTrigger operator !(AiTrigger t) => new NotTrigger(t);
    
    private class AndTrigger : CompositeTrigger
    {
        public AndTrigger(AiTrigger t1, AiTrigger t2) : base(t1, t2) {}

        public override bool Triggers() => t1.Triggers() && t2.Triggers();
    }
    
    private class OrTrigger : CompositeTrigger
    {
        public OrTrigger(AiTrigger t1, AiTrigger t2) : base(t1, t2) {}

        public override bool Triggers() => t1.Triggers() || t2.Triggers();
    }

    private class XOrTrigger : CompositeTrigger
    {
        public XOrTrigger(AiTrigger t1, AiTrigger t2) : base(t1, t2) {}

        public override bool Triggers() => t1.Triggers() ^ t2.Triggers();
    }

    private class NotTrigger : AiTrigger
    {
        private readonly AiTrigger trigger;

        public NotTrigger(AiTrigger trigger)
        {
            this.trigger = trigger;
        }

        public override void Activate() => trigger.Activate();
        public override void Deactivate() => trigger.Deactivate();
        public override bool Triggers() => !trigger.Triggers();
    }

    private abstract class CompositeTrigger : AiTrigger
    {
        protected readonly AiTrigger t1;
        protected readonly AiTrigger t2;

        protected CompositeTrigger(AiTrigger t1, AiTrigger t2)
        {
            this.t1 = t1;
            this.t2 = t2;
        }

        public override void Activate()
        {
            t1.Activate();
            t2.Activate();
        }

        public override void Deactivate()
        {
            t1.Deactivate();
            t2.Deactivate();
        }
    }
}
