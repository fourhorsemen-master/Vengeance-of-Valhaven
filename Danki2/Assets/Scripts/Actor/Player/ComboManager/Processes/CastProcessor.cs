using System;

public class CastProcessor : Processor<ComboState>
{
    private Player player;
    private readonly Action<bool?> setFeedback;
    private Direction direction;
    private Subscription<bool> feedbackSubscription;

    public CastProcessor(Player player, Action<bool?> setFeedback, Direction direction)
    {
        this.player = player;
        this.setFeedback = setFeedback;
        this.direction = direction;
    }

    public override void Enter()
    {
        setFeedback(null);
    }

    public override void Exit()
    {
    }

    public override bool TryCompleteProcess(out ComboState nextState)
    {
        AbilityReference abilityReference = player.AbilityTree.GetAbility(direction);
        AbilityType abilityType = AbilityLookup.Instance.GetAbilityType(abilityReference);

        switch (abilityType)
        {
            case AbilityType.InstantCast:
                bool hasCast = player.InstantCastService.TryCast(
                    abilityReference,
                    player.TargetFinder.FloorTargetPosition,
                    player.TargetFinder.OffsetTargetPosition,
                    subject => SubscribeToFeedback(subject),
                    player.TargetFinder.Target
                );
                if (hasCast)
                {
                    nextState = ComboState.AwaitingFeedback;
                    return true;
                }
                break;
            case AbilityType.Channel:
                bool hasStartedChannel = player.ChannelService.TryStartChannel(
                    abilityReference,
                    subject => SubscribeToFeedback(subject)
                );
                if (hasStartedChannel)
                {
                    nextState = direction == Direction.Left ? ComboState.ChannelingLeft : ComboState.ChannelingRight;
                    return true;
                }
                break;
        }

        nextState = player.AbilityTree.AtRoot ? ComboState.ReadyAtRoot : ComboState.ReadyInCombo;
        return false;
    }

    private void SubscribeToFeedback(Subject<bool> feedbackSubject)
    {
        feedbackSubscription = feedbackSubject.Subscribe(feedback =>
        {
            feedbackSubscription.Unsubscribe();
            setFeedback(feedback);
        });
    }
}