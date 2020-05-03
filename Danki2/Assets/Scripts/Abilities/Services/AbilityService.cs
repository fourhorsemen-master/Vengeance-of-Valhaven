using System.Collections.Generic;

public abstract class AbilityService
{
    protected readonly Actor actor;

    private readonly List<IAbilityDataDiffer> differs = new List<IAbilityDataDiffer>();

    protected AbilityService(Actor actor)
    {
        this.actor = actor;
    }

    public void RegisterAbilityDataDiffer(IAbilityDataDiffer differ)
    {
        differs.Add(differ);
    }

    protected AbilityData GetAbilityDataDiff(AbilityReference abilityReference)
    {
        return AbilityData.FromAbilityDataDiffers(differs, abilityReference);
    }
}
