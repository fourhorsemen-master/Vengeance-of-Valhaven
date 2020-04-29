using System;
using System.Collections.Generic;

public class AbilityDataOrbsDiffer : IAbilityDataDiffer
{
    private Node currentNode;

    public AbilityDataOrbsDiffer(AbilityTree abilityTree)
    {
        abilityTree.TreeWalkSubject.Subscribe(n => currentNode = n);
    }

    public AbilityData GetAbilityDataDiff()
    {
        EnumDictionary<OrbType, int> totalGeneratedOrbs = new EnumDictionary<OrbType, int>(0);

        currentNode.IterateUp(
            node =>
            {
                Dictionary<OrbType, int> generatedOrbs = AbilityLookup.GetGeneratedOrbs(node.Ability);
                foreach (OrbType orbType in Enum.GetValues(typeof(OrbType)))
                {
                    if (generatedOrbs.TryGetValue(orbType, out int orbCount)) totalGeneratedOrbs[orbType] += orbCount;
                }
            },
            node => !node.IsRootNode()
        );

        return AbilityData.FromGeneratedOrbs(totalGeneratedOrbs);
    }
}
