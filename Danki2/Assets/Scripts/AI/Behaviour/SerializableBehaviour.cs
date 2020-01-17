using System;

[Serializable]
public class SerializableBehaviour : SerializableAIElement<Behaviour>
{
    private Type _noOpType = typeof(NoOpBehaviour);
    protected override Type NoOpType => _noOpType;

    public SerializableBehaviour() : base()
    {
    }

    public SerializableBehaviour(Type plannerType, float[] args) : base(plannerType, args)
    {
    }
}
