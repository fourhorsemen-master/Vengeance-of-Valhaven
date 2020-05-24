public class AbilityBonusNoOpCalculator : IAbilityBonusCalculator
{
    public string[] GetActiveBonuses(AbilityReference abilityReference)
    {
        return new string[0];
    }
}
