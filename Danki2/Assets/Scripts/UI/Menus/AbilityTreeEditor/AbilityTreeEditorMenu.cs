using UnityEngine;

public class AbilityTreeEditorMenu : Singleton<AbilityTreeEditorMenu>
{
    private void Start()
    {
        GameStateController.Instance.GameStateTransitionSubject.Subscribe(gameState =>
                gameObject.SetActive(gameState == GameState.InAbilityTreeEditor)
        );
    }
}
