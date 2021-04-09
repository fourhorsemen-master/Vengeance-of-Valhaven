using System.Collections.Generic;
using UnityEngine;

public class MenuController : Singleton<MenuController>
{
    [SerializeField] private AbilityTreeEditorMenu abilityTreeEditorMenu = null;
    [SerializeField] private AbilitySelectionMenu abilitySelectionMenu = null;
    [SerializeField] private PauseMenu pauseMenu = null;

    private Dictionary<GameplayState, GameObject> menuLookup = new Dictionary<GameplayState, GameObject>();
    
    private void Start()
    {
        menuLookup = new Dictionary<GameplayState, GameObject>
        {
            [GameplayState.InAbilityTreeEditor] = abilityTreeEditorMenu.gameObject,
            [GameplayState.InAbilitySelection] = abilitySelectionMenu.gameObject,
            [GameplayState.InPauseMenu] = pauseMenu.gameObject,
        };

        GameplayStateController.Instance.GameStateTransitionSubject.Subscribe(newGameplayState =>
        {
            foreach (GameplayState gameplayState in menuLookup.Keys)
            {
                if (!menuLookup.ContainsKey(gameplayState)) return;
                menuLookup[gameplayState].SetActive(gameplayState == newGameplayState);
            }
        });
    }
}
