using System.Collections.Generic;
using System.Linq;

public class AbilityBonusOrbsCalculator : IAbilityBonusCalculator
{
    private Node currentNode;

    public AbilityBonusOrbsCalculator(AbilityTree abilityTree)
    {
        abilityTree.TreeWalkSubject.Subscribe(n => currentNode = n);
    }
    
    /// <summary>
    /// Calculates the active bonuses for the given ability if it were cast from the current ability tree position
    /// </summary>
    /// <param name="abilityReference"> The ability to get the bonuses for. </param>
    /// <returns> An array of the active bonuses. </returns>
    public string[] GetActiveBonuses(AbilityReference abilityReference)
    {
        OrbCollection activeOrbs = GetActiveOrbs();

        Dictionary<string, AbilityBonusData> abilityBonusDataLookup = AbilityLookup.Instance.GetAbilityBonusDataLookup(abilityReference);

        return abilityBonusDataLookup.Keys
            .Where(abilityBonus => activeOrbs.IsSuperset(abilityBonusDataLookup[abilityBonus].RequiredOrbs))
            .ToArray();
    }

    private OrbCollection GetActiveOrbs()
    {
        OrbCollection activeOrbs = new OrbCollection();
        
        currentNode.IterateUp(
            n => activeOrbs.Add(AbilityLookup.Instance.GetGeneratedOrbs(n.Ability)),
            n => !n.IsRootNode()
        );

        return activeOrbs;
    }
}
