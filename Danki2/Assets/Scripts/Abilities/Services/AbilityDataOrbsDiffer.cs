using System.Collections.Generic;

public class AbilityDataOrbsDiffer : IAbilityDataDiffer
{
    private Node currentNode;

    public AbilityDataOrbsDiffer(AbilityTree abilityTree)
    {
        abilityTree.TreeWalkSubject.Subscribe(n => currentNode = n);
    }

    public AbilityData GetAbilityDataDiff(AbilityReference abilityReference)
    {
        OrbType abilityOrbType = AbilityLookup.GetAbilityOrbType(abilityReference);
        int totalOrbCount = 0;

        currentNode.IterateUp(
            node =>
            {
                Dictionary<OrbType, int> generatedOrbs = AbilityLookup.GetGeneratedOrbs(node.Ability);
                if (generatedOrbs.TryGetValue(abilityOrbType, out int orbCount)) totalOrbCount += orbCount;
            },
            node => !node.IsRootNode()
        );

        return AbilityData.FromOrbCount(abilityOrbType, totalOrbCount);
    }
}
