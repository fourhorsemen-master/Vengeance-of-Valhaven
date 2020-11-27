using System.Collections.Generic;

public abstract class AbilityService
{
    protected readonly Actor actor;
    protected float feedbackTimeout = 1f;
    private readonly List<IAbilityDataDiffer> differs = new List<IAbilityDataDiffer>();
    private bool subscribedToFeedback;
    private Subscription<bool> feedbackSubscription;
    private IAbilityBonusCalculator abilityBonusCalculator = new AbilityBonusNoOpCalculator();
    public Subject<bool> FeedbackSubject  = new Subject<bool>();

    public bool CanCast => !actor.Dead
        && !actor.MovementManager.Stunned
        && !actor.MovementManager.MovementLocked;

    protected AbilityService(Actor actor)
    {
        this.actor = actor;        
    }

    public void SetFeedbackTimeout(float feedbackTimeout)
    {
        this.feedbackTimeout = feedbackTimeout;
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

    protected void SubscribeToFeedback(Ability ability)
    {
        subscribedToFeedback = true;

        feedbackSubscription = ability.SuccessFeedbackSubject.Subscribe(feedback =>
        {
            feedbackSubscription.Unsubscribe();
            FeedbackSubject.Next(feedback);
            subscribedToFeedback = false;
        });
    }

    protected void StartFeedbackTimer()
    {
        if (!subscribedToFeedback) return;

        actor.WaitAndAct(feedbackTimeout, () =>
        {
            if (subscribedToFeedback)
            {
                feedbackSubscription.Unsubscribe();
                FeedbackSubject.Next(false);
                subscribedToFeedback = false;
            }
        });
    }
}
