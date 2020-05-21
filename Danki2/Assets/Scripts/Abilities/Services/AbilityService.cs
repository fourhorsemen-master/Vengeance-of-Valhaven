using System.Collections.Generic;

public abstract class AbilityService
{
    protected readonly Actor actor;

    private readonly List<IAbilityDataDiffer> differs = new List<IAbilityDataDiffer>();
    private IAbilityBonusCalculator abilityBonusCalculator = new NoOpAbilityBonusCalculator();

    protected AbilityService(Actor actor)
    {
        this.actor = actor;
    }

    public void RegisterAbilityDataDiffer(IAbilityDataDiffer differ)
    {
        differs.Add(differ);
    }

    public void ReplaceAbilityBonusCalculator(IAbilityBonusCalculator replacingAbilityBonusCalculator)
    {
        abilityBonusCalculator = replacingAbilityBonusCalculator;
    }

    protected AbilityData GetAbilityDataDiff(AbilityReference abilityReference)
    {
        return AbilityData.FromAbilityDataDiffers(differs, abilityReference);
    }

    protected string[] GetActiveBonuses(AbilityReference abilityReference)
    {
        return abilityBonusCalculator.GetActiveBonuses(abilityReference);
    }
}
