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
        return new StateMachine<GameplayState>(GameplayState.Playing)
            .WithComponent(GameplayState.Playing, new GameplayStateComponent(GameplayState.Playing))
            .WithComponent(GameplayState.InPauseMenu, new GameplayStateComponent(GameplayState.InPauseMenu))
            .WithComponent(GameplayState.InAbilityTreeEditor, new GameplayStateComponent(GameplayState.InAbilityTreeEditor))
            .WithComponent(GameplayState.InAbilitySelection, new GameplayStateComponent(GameplayState.InAbilitySelection))
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
                new ShrineExists() & new CanInteractWithShrine() & new ButtonDown(AbilitySelectionButtonName)
            )
            .WithTransition(
                GameplayState.InPauseMenu,
                GameplayState.Playing,
                new ButtonDown(PauseButtonName) | new SubjectEmitted(pauseMenu.ContinueClickedSubject))
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
                new SubjectEmitted(abilitySelectionMenu.ConfirmClickedSubject) |
                new ButtonDown(AbilitySelectionButtonName)
            )
            .WithTransition(
                GameplayState.InAbilitySelection,
                GameplayState.InPauseMenu,
                new ButtonDown(PauseButtonName)
            );
    }
}
