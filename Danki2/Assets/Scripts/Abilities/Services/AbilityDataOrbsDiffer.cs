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
        OrbType? abilityOrbType = AbilityMetadataLookup.Instance.GetAbilityOrbType(abilityReference);
        return abilityOrbType.HasValue
            ? GetOrbCount(currentNode, abilityOrbType.Value) * AbilityData.One
            : AbilityData.Zero;
    }

    public AbilityData GetAbilityDataDiff(Node node)
    {
        if (node.IsRootNode()) return AbilityData.Zero;
        
        OrbType? abilityOrbType = AbilityMetadataLookup.Instance.GetAbilityOrbType(node.Ability);
        return abilityOrbType.HasValue
            ? GetOrbCount(node.Parent, abilityOrbType.Value) * AbilityData.One
            : AbilityData.Zero;
    }

    private int GetOrbCount(Node startingNode, OrbType abilityOrbType)
    {
        int totalOrbCount = 0;
        
        startingNode.IterateUp(
            node =>
            {
                Dictionary<OrbType, int> generatedOrbs = AbilityMetadataLookup.Instance.GetGeneratedOrbs(node.Ability);
                if (generatedOrbs.TryGetValue(abilityOrbType, out int orbCount)) totalOrbCount += orbCount;
            },
            ancestorNode => !ancestorNode.IsRootNode()
        );

        return totalOrbCount;
    }
}
