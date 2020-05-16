using System;
using System.Collections.Generic;
using System.Linq;

public class OrbCollection : EnumDictionary<OrbType, int>
{
    public OrbCollection() : base(0)
    {
    }

    public OrbCollection(List<OrbType> orbTypes) : base (0)
    {
        foreach (OrbType key in Enum.GetValues(typeof(OrbType)))
        {
            this[key] = orbTypes.Count(o => o == key);
        }
    }

    public bool IsEmpty()
    {
        return Values.All(v => v == 0);
    }
}
