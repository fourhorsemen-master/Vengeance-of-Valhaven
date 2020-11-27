using System.Collections.Generic;
using System.Linq;

public class AbilityBonusTreeDepthCalculator : IAbilityBonusCalculator
{
    private int currentDepth;

    public AbilityBonusTreeDepthCalculator(AbilityTree abilityTree)
    {
        abilityTree.CurrentDepthSubject.Subscribe(d => currentDepth = d);
    }

    /// <summary>
    /// Calculates the active bonuses for the given ability if it were cast from the current ability tree position.
    /// </summary>
    /// <param name="abilityReference"> The ability to get the bonuses for. </param>
    public string[] GetActiveBonuses(AbilityReference abilityReference)
    {
        Dictionary<string, AbilityBonusData> abilityBonusDataLookup =
            AbilityLookup.Instance.GetAbilityBonusDataLookup(abilityReference);

        return abilityBonusDataLookup.Keys
            .Where(abilityBonus => abilityBonusDataLookup[abilityBonus].RequiredTreeDepth <= currentDepth + 1)
            .ToArray();
    }
}
