using System;

public class OrbCollection : EnumDictionary<OrbType, int>
{
    public OrbCollection() : base(0)
    {
    }

    public bool IsEmpty()
    {
        foreach (OrbType key in Enum.GetValues(typeof(OrbType)))
        {
            if (TryGetValue(key, out int value) && value > 0) return false;
        }

        return true;
    }
}