public class NoOpAbilityBonusCalculator : IAbilityBonusCalculator
{
    public string[] GetActiveBonuses(AbilityReference abilityReference)
    {
        return new string[0];
    }
}
