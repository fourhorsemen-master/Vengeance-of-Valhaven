public abstract class StateMachineTrigger
{
    public abstract void Activate();
    public abstract void Deactivate();
    public abstract bool Triggers();

    public static StateMachineTrigger operator &(StateMachineTrigger t1, StateMachineTrigger t2) => new AndTrigger(t1, t2);
    public static StateMachineTrigger operator |(StateMachineTrigger t1, StateMachineTrigger t2) => new OrTrigger(t1, t2);
    public static StateMachineTrigger operator ^(StateMachineTrigger t1, StateMachineTrigger t2) => new XOrTrigger(t1, t2);
    public static StateMachineTrigger operator !(StateMachineTrigger t) => new NotTrigger(t);
    
    private class AndTrigger : CompositeTrigger
    {
        public AndTrigger(StateMachineTrigger t1, StateMachineTrigger t2) : base(t1, t2) {}

        public override bool Triggers() => t1.Triggers() && t2.Triggers();
    }
    
    private class OrTrigger : CompositeTrigger
    {
        public OrTrigger(StateMachineTrigger t1, StateMachineTrigger t2) : base(t1, t2) {}

        public override bool Triggers() => t1.Triggers() || t2.Triggers();
    }

    private class XOrTrigger : CompositeTrigger
    {
        public XOrTrigger(StateMachineTrigger t1, StateMachineTrigger t2) : base(t1, t2) {}

        public override bool Triggers() => t1.Triggers() ^ t2.Triggers();
    }

    private class NotTrigger : StateMachineTrigger
    {
        private readonly StateMachineTrigger stateMachineTrigger;

        public NotTrigger(StateMachineTrigger stateMachineTrigger)
        {
            this.stateMachineTrigger = stateMachineTrigger;
        }

        public override void Activate() => stateMachineTrigger.Activate();
        public override void Deactivate() => stateMachineTrigger.Deactivate();
        public override bool Triggers() => !stateMachineTrigger.Triggers();
    }

    private abstract class CompositeTrigger : StateMachineTrigger
    {
        protected readonly StateMachineTrigger t1;
        protected readonly StateMachineTrigger t2;

        protected CompositeTrigger(StateMachineTrigger t1, StateMachineTrigger t2)
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
