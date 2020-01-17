using System;

[Serializable]
public class SerializablePlanner : SerializableAIElement<Planner>
{
    private Type _noOpType = typeof(NoOpPlanner);
    protected override Type NoOpType => _noOpType;

    public SerializablePlanner() : base()
    {
    }

    public SerializablePlanner(Type plannerType, float[] args) : base(plannerType, args)
    {
    }
}
