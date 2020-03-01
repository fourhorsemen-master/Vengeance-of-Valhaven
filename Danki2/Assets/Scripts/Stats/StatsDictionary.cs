using System;

[Serializable]
public class StatsDictionary : SerializableEnumDictionary<Stat, int>
{
    public StatsDictionary(int defaultValue) : base(defaultValue)
    {
    }

    public StatsDictionary(SerializableEnumDictionary<Stat, int> dictionary) : base(dictionary)
    {
    }
}

