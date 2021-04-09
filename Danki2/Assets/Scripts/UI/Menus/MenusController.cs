using UnityEngine;

public class MenusController : Singleton<MenusController>
{
    [SerializeField] private AbilityTreeEditorMenu abilityTreeEditorMenu = null;
    [SerializeField] private AbilitySelectionMenu abilitySelectionMenu = null;
    [SerializeField] private PauseMenu pauseMenu = null;

    private void Start()
    {
        GameplayStateController.Instance.GameStateTransitionSubject.Subscribe(gameplayState =>
        {
            switch (gameplayState)
            {
                case GameplayState.InAbilityTreeEditor:
                    abilityTreeEditorMenu.gameObject.SetActive(true);
                    abilitySelectionMenu.gameObject.SetActive(false);
                    pauseMenu.gameObject.SetActive(false);
                    break;
                case GameplayState.InAbilitySelection:
                    abilityTreeEditorMenu.gameObject.SetActive(false);
                    abilitySelectionMenu.gameObject.SetActive(true);
                    pauseMenu.gameObject.SetActive(false);
                    break;
                case GameplayState.InPauseMenu:
                    abilityTreeEditorMenu.gameObject.SetActive(false);
                    abilitySelectionMenu.gameObject.SetActive(false);
                    pauseMenu.gameObject.SetActive(true);
                    break;
                default:
                    abilityTreeEditorMenu.gameObject.SetActive(false);
                    abilitySelectionMenu.gameObject.SetActive(false);
                    pauseMenu.gameObject.SetActive(false);
                    break;
            }
        });
    }
}
