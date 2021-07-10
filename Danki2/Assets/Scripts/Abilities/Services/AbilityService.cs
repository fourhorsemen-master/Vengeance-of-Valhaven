﻿using System.Collections.Generic;

public abstract class AbilityService
{
    protected readonly Player player;
    
    private readonly List<IAbilityDataDiffer> differs = new List<IAbilityDataDiffer>();
    private IAbilityBonusCalculator abilityBonusCalculator = new AbilityBonusNoOpCalculator();
    public Subject<bool> FeedbackSubject { get; } = new Subject<bool>();

    protected AbilityService(Player player)
    {
        this.player = player;        
    }

    public void RegisterAbilityDataDiffer(IAbilityDataDiffer differ)
    {
        differs.Add(differ);
    }

    public void SetAbilityBonusCalculator(IAbilityBonusCalculator replacingAbilityBonusCalculator)
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
