public interface IAbilityDataDiffer
{
    AbilityData GetAbilityDataDiff(AbilityReference abilityReference);
    AbilityData GetAbilityDataDiff(Node node);
}
