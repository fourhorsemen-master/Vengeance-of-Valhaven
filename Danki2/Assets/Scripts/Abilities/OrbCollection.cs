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
        EnumUtils.ForEach<OrbType>(type => this[type] = orbTypes.Count(o => o == type));
    }

    public bool IsSuperset(OrbCollection other)
    {
        return Enum.GetValues(typeof(OrbType))
            .Cast<OrbType>()
            .All(orbType => this[orbType] >= other[orbType]);
    }

    public void Add(OrbCollection other)
    {
        EnumUtils.ForEach<OrbType>(type => this[type] += other[type]);
    }

    /// <summary>
    /// Loops through the OrbType values and calls the given action once for each orb within that type.
    /// </summary>
    /// <param name="action"></param>
    public void ForEachOrb(Action<OrbType> action)
    {
        EnumUtils.ForEach<OrbType>(type =>
        {
            for (int i = 0; i < this[type]; i++)
            {
                action(type);
            }
        });
    }
}
