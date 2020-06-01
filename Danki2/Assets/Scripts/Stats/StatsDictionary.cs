using System;

[Serializable]
public class StatsDictionary : SerializableEnumDictionary<Stat, int>
{
    public StatsDictionary(int defaultValue) : base(defaultValue)
    {
    }

    public StatsDictionary(Func<int> defaultValueProvider) : base(defaultValueProvider)
    {
    }
}

