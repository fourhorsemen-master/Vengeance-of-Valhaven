using System.Collections.Generic;
using System.Linq;

public class AbilityBonusTreeDepthCalculator : IAbilityBonusCalculator
{
    private readonly AbilityTree abilityTree;

    public AbilityBonusTreeDepthCalculator(AbilityTree abilityTree)
    {
        this.abilityTree = abilityTree;
    }

    /// <summary>
    /// Calculates the active bonuses for the given ability as if it was on the active node.
    /// </summary>
    /// <param name="abilityReference"> The ability to get the bonuses for. </param>
    public string[] GetActiveBonuses(AbilityReference abilityReference)
    {
        Dictionary<string, AbilityBonusData> abilityBonusDataLookup =
            AbilityLookup.Instance.GetAbilityBonusDataLookup(abilityReference);

        return abilityBonusDataLookup.Keys
            .Where(abilityBonus => abilityBonusDataLookup[abilityBonus].RequiredTreeDepth <= abilityTree.CurrentDepth)
            .ToArray();
    }
}
