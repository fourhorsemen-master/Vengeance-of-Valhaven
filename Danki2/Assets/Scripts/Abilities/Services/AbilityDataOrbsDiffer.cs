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
        return GetOrbCount(currentNode, abilityOrbType) * AbilityData.One;
    }

    public AbilityData GetAbilityDataDiff(Node node)
    {
        if (node.IsRootNode()) return AbilityData.Zero;
        
        OrbType abilityOrbType = AbilityLookup.GetAbilityOrbType(node.Ability);
        return GetOrbCount(node.Parent, abilityOrbType) * AbilityData.One;
    }

    private int GetOrbCount(Node startingNode, OrbType abilityOrbType)
    {
        int totalOrbCount = 0;
        
        startingNode.IterateUp(
            node =>
            {
                OrbCollection generatedOrbs = AbilityLookup.GetGeneratedOrbs(node.Ability);
                if (generatedOrbs.TryGetValue(abilityOrbType, out int orbCount)) totalOrbCount += orbCount;
            },
            ancestorNode => !ancestorNode.IsRootNode()
        );

        return totalOrbCount;
    }
}
