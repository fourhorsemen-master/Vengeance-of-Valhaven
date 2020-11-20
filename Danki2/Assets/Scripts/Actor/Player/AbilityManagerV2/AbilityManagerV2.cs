using UnityEngine;

public class AbilityManagerV2 : StateMachineMonoBehaviour
{
    [SerializeField]
    private Player player = null;

    [Header("Ability tree")]
    [SerializeField] private float shortCooldown = 0.75f;
    [SerializeField] private float longCooldown = 1.5f;
    [SerializeField] private float comboTimeout = 2f;
    [SerializeField] private float feedbackTimeout = 1f;
    [SerializeField] private bool rollResetsCombo = false;

    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        StateMachine<State> stateMachine = new StateMachine<State>(State.ReadyAtRoot)
            .WithComponent(State.ReadyAtRoot, new ListenForCasts(player))
            .WithComponent(State.ReadyInCombo, new ListenForCasts(player))
            .WithComponent(State.ChannelingLeft, new ListenForChannelEnd(player, Direction.Left))
            .WithComponent(State.ChannelingRight, new ListenForChannelEnd(player, Direction.Right))
            .WithComponent(State.AwaitingFeedback, new NoOpComponent())
            .WithComponent(State.Whiff, new Whiff(player))
            .WithComponent(State.LongCooldown, new NoOpComponent())
            .WithComponent(State.ShortCooldown, new NoOpComponent())
            .WithTransition(State.ReadyAtRoot, State.AwaitingFeedback, new InstantCastTrigger(player))
            .WithTransition(State.ReadyAtRoot, State.ChannelingRight, new ChannelStartTrigger(player, Direction.Right))
            .WithTransition(State.ReadyAtRoot, State.ChannelingLeft, new ChannelStartTrigger(player, Direction.Left))
            .WithTransition(State.ReadyInCombo, State.AwaitingFeedback, new InstantCastTrigger(player))
            .WithTransition(State.ReadyInCombo, State.ChannelingRight, new ChannelStartTrigger(player, Direction.Right))
            .WithTransition(State.ReadyInCombo, State.ChannelingLeft, new ChannelStartTrigger(player, Direction.Left))
            .WithTransition(State.ReadyInCombo, State.Whiff, new TimeElapsed(comboTimeout))
            .WithTransition(State.ChannelingLeft, State.AwaitingFeedback, new ChannelEndTrigger(player))
            .WithTransition(State.ChannelingRight, State.AwaitingFeedback, new ChannelEndTrigger(player))
            .WithTransition(State.AwaitingFeedback, State.Whiff, new TimeElapsed(feedbackTimeout))
            .WithTransition(State.AwaitingFeedback, State.ShortCooldown, new ComboContinueTrigger(player))
            .WithTransition(State.AwaitingFeedback, State.LongCooldown, new ComboCompleteTrigger(player))
            .WithTransition(State.AwaitingFeedback, State.LongCooldown, new ComboFailedTrigger(player))
            .WithTransition(State.Whiff, State.LongCooldown, new AlwaysTrigger())
            .WithTransition(State.ShortCooldown, State.ReadyInCombo, new TimeElapsed(shortCooldown))
            .WithTransition(State.LongCooldown, State.ReadyAtRoot, new TimeElapsed(longCooldown))
            .WithGlobalTransition(State.Whiff, !new AtRootTrigger(player) & new DamageTrigger(player));

        if (rollResetsCombo)
        {
            stateMachine = stateMachine
                .WithGlobalTransition(State.Whiff, !new AtRootTrigger(player) & new RollTrigger(player));
        }

        return stateMachine;
    }

    private enum State
    {
        ReadyAtRoot,
        ReadyInCombo,
        ChannelingRight,
        ChannelingLeft,
        AwaitingFeedback,
        Whiff,
        LongCooldown,
        ShortCooldown
    }
}
