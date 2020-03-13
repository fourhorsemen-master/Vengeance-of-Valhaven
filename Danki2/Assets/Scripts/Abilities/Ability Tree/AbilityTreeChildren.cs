using System;

[Serializable]
public class AbilityTreeChildren : SerializableEnumDictionary<Direction, Node>
{
    public AbilityTreeChildren(Node defaultValue) : base(defaultValue)
    {
    }

    public AbilityTreeChildren(SerializableEnumDictionary<Direction, Node> dictionary) : base(dictionary)
    {
    }
}
