using System;

public class PassiveEffectData
{
    public Guid Id { get; }
    public PassiveEffect Effect { get; }

    public PassiveEffectData(Guid id, PassiveEffect effect)
    {
        Id = id;
        Effect = effect;
    }
}
