using System;
using System.Collections.Generic;
using System.Linq;

public class OrbCollection : EnumDictionary<OrbType, int>
{
    public bool IsEmpty => Values.All(v => v == 0);

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

    public bool IsSuperset(OrbCollection other)
    {
        return Enum.GetValues(typeof(OrbType))
            .Cast<OrbType>()
            .All(orbType => this[orbType] >= other[orbType]);
    }

    public void Add(OrbCollection other)
    {
        foreach (OrbType orbType in Enum.GetValues(typeof(OrbType)))
        {
            this[orbType] += other[orbType];
        }
    }

    /// <summary>
    /// Loops through the OrbType values and calls the given action once for each orb within that type.
    /// </summary>
    /// <param name="action"></param>
    public void ForEachOrb(Action<OrbType> action)
    {
        foreach (OrbType key in Enum.GetValues(typeof(OrbType)))
        {
            for (int i = 0; i < this[key]; i++)
            {
                action(key);
            }
        }
    }
}
