﻿using System.Collections.Generic;

public abstract class AbilityService
{
<<<<<<< HEAD
    protected readonly Actor actor;
=======
    protected readonly Player player;
    private float feedbackTimeout = 1f;
>>>>>>> split-enemy-and-player-abilities
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
<<<<<<< HEAD
=======

    protected void SubscribeToFeedback(Ability ability)
    {
        subscribedToFeedback = true;

        feedbackSubscription = ability.SuccessFeedbackSubject.Subscribe(feedback =>
        {
            feedbackSubscription.Unsubscribe();
            StopFeedbackTimer();
            FeedbackSubject.Next(feedback);
            subscribedToFeedback = false;
        });
    }

    protected void StartFeedbackTimer()
    {
        if (!subscribedToFeedback) return;

        feedbackTimer = player.WaitAndAct(feedbackTimeout, () =>
        {
            feedbackSubscription.Unsubscribe();
            StopFeedbackTimer();
            FeedbackSubject.Next(false);
            subscribedToFeedback = false;
        });
    }

    private void StopFeedbackTimer()
    {
        if (feedbackTimer == null) return;

        player.StopCoroutine(feedbackTimer);
        feedbackTimer = null;
    }
>>>>>>> split-enemy-and-player-abilities
}
