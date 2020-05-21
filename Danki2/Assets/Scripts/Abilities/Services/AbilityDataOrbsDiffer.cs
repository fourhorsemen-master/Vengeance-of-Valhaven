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
        return AbilityLookup.Instance.TryGetAbilityOrbType(abilityReference, out OrbType orbType)
            ? GetOrbCount(currentNode, orbType) * AbilityData.One
            : AbilityData.Zero;
    }

    public AbilityData GetAbilityDataDiff(Node node)
    {
        if (node.IsRootNode()) return AbilityData.Zero;
        
        return AbilityLookup.Instance.TryGetAbilityOrbType(node.Ability, out OrbType orbType)
            ? GetOrbCount(node.Parent, orbType) * AbilityData.One
            : AbilityData.Zero;
    }

    private int GetOrbCount(Node startingNode, OrbType abilityOrbType)
    {
        int totalOrbCount = 0;
        
        startingNode.IterateUp(
            node =>
            {
                Dictionary<OrbType, int> generatedOrbs = AbilityLookup.Instance.GetGeneratedOrbs(node.Ability);
                if (generatedOrbs.TryGetValue(abilityOrbType, out int orbCount)) totalOrbCount += orbCount;
            },
            ancestorNode => !ancestorNode.IsRootNode()
        );

        return totalOrbCount;
    }
}
