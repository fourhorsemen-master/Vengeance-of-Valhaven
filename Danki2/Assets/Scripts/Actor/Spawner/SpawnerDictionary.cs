using System;

[Serializable]
public class SpawnerDictionary : SerializableEnumDictionary<ActorType, Actor>
{
    public SpawnerDictionary(Actor defaultValue) : base(defaultValue)
    {
    }

    public SpawnerDictionary(Func<Actor> defaultValueProvider) : base(defaultValueProvider)
    {
    }
}
