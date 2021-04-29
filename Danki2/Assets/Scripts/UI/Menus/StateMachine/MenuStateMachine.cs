using UnityEngine;

public class MenuStateMachine : StateMachineMonoBehaviour
{
    private const string PauseButtonName = "PauseMenu";
    private const string AbilityTreeButtonName = "AbilityTreeMenu";
    private const string AbilitySelectionButtonName = "Interact";
    private const string RuneMenuButtonName = "RuneMenu";
    
    [SerializeField] private PauseMenu pauseMenu = null;
    [SerializeField] private AbilitySelectionMenu abilitySelectionMenu = null;
    
    protected override IStateMachineComponent BuildStateMachineComponent()
    {
        return new StateMachine<GameplayState>(GameplayState.Playing)
            .WithComponent(GameplayState.Playing, new GameplayStateComponent(GameplayState.Playing))
            .WithComponent(GameplayState.InPauseMenu, new GameplayStateComponent(GameplayState.InPauseMenu))
            .WithComponent(GameplayState.InAbilityTreeEditor, new GameplayStateComponent(GameplayState.InAbilityTreeEditor))
            .WithComponent(GameplayState.InAbilitySelection, new GameplayStateComponent(GameplayState.InAbilitySelection))
            .WithComponent(GameplayState.InRuneMenu, new GameplayStateComponent(GameplayState.InRuneMenu))
            .WithTransition(
                GameplayState.Playing,
                GameplayState.InPauseMenu,
                new ButtonDown(PauseButtonName)
            )
            .WithTransition(
                GameplayState.Playing,
                GameplayState.InAbilityTreeEditor,
                new ButtonDown(AbilityTreeButtonName)
            )
            .WithTransition(
                GameplayState.Playing,
                GameplayState.InAbilitySelection,
                new ButtonDown(AbilitySelectionButtonName) & new ShrineExists() & new CanInteractWithShrine()
            )
            .WithTransition(
                GameplayState.Playing,
                GameplayState.InRuneMenu,
                new ButtonDown(RuneMenuButtonName)
            )
            .WithTransition(
                GameplayState.InPauseMenu,
                GameplayState.Playing,
                new ButtonDown(PauseButtonName) | new SubjectEmitted(pauseMenu.ContinueClickedSubject)
            )
            .WithTransition(
                GameplayState.InAbilityTreeEditor,
                GameplayState.Playing,
                new ButtonDown(AbilityTreeButtonName)
            )
            .WithTransition(
                GameplayState.InAbilityTreeEditor,
                GameplayState.InPauseMenu,
                new ButtonDown(PauseButtonName)
            )
            .WithTransition(
                GameplayState.InAbilitySelection,
                GameplayState.Playing,
                new SubjectEmitted(abilitySelectionMenu.SkipClickedSubject) |
                new SubjectEmitted(abilitySelectionMenu.ConfirmClickedSubject)
            )
            .WithTransition(
                GameplayState.InAbilitySelection,
                GameplayState.InPauseMenu,
                new ButtonDown(PauseButtonName)
            )
            .WithTransition(
                GameplayState.InRuneMenu,
                GameplayState.Playing,
                new ButtonDown(RuneMenuButtonName)
            );
    }
}
