using System;

[Serializable]
public class SerializablePersonality : SerializableEnumDictionary<AIAction, SerializableBehaviour>
{
    public SerializablePersonality(SerializableBehaviour defaultValue) : base(defaultValue)
    {
    }

    public SerializablePersonality(SerializableEnumDictionary<AIAction, SerializableBehaviour> dictionary) : base(dictionary)
    {
    }
}
