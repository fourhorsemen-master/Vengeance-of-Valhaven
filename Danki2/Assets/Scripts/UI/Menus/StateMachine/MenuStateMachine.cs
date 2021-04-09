using UnityEngine;

public class MenuStateMachine : StateMachineMonoBehaviour
{
    private const string PauseButtonName = "PauseMenu";
    private const string AbilityTreeButtonName = "AbilityTreeMenu";
    private const string AbilitySelectionButtonName = "Interact";
    
    [SerializeField] private PauseMenu pauseMenu = null;
    [SerializeField] private AbilitySelectionMenu abilitySelectionMenu = null;
    
    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        return new StateMachine<State>(State.Playing)
            .WithComponent(State.Playing, new GameplayStateComponent(GameplayState.Playing))
            .WithComponent(State.Paused, new GameplayStateComponent(GameplayState.InPauseMenu))
            .WithComponent(State.AbilityTree, new GameplayStateComponent(GameplayState.InAbilityTreeEditor))
            .WithComponent(State.AbilitySelection, new GameplayStateComponent(GameplayState.InAbilitySelection))
            .WithTransition(State.Playing, State.Paused, new ButtonDown(PauseButtonName))
            .WithTransition(State.Playing, State.AbilityTree, new ButtonDown(AbilityTreeButtonName))
            .WithTransition(
                State.Playing,
                State.AbilitySelection,
                new ShrineExists() & new CanInteractWithShrine() & new ButtonDown(AbilitySelectionButtonName)
            )
            .WithTransition(State.Paused, State.Playing, new ButtonDown(PauseButtonName) | new SubjectEmitted(pauseMenu.ContinueClickedSubject))
            .WithTransition(State.AbilityTree, State.Playing, new ButtonDown(AbilityTreeButtonName))
            .WithTransition(State.AbilityTree, State.Paused, new ButtonDown(PauseButtonName))
            .WithTransition(
                State.AbilitySelection,
                State.Playing,
                new SubjectEmitted(abilitySelectionMenu.SkipClickedSubject) | new SubjectEmitted(abilitySelectionMenu.ConfirmClickedSubject) | new ButtonDown(AbilitySelectionButtonName)
            )
            .WithTransition(State.AbilitySelection, State.Paused, new ButtonDown(PauseButtonName));
    }

    private enum State
    {
        Playing,
        Paused,
        AbilityTree,
        AbilitySelection
    }
}
