using UnityEngine;

public class AbilityTreeEditorMenu : Singleton<AbilityTreeEditorMenu>
{
    [SerializeField]
    private AbilityTreeDisplay abilityTreeDisplay = null;

    private void Start()
    {
        GameStateController.Instance.GameStateTransitionSubject.Subscribe(gameState =>
        {
            if (gameState == GameState.InAbilityTreeEditor)
            {
                gameObject.SetActive(true);

                abilityTreeDisplay.RecalculateDisplay();
            }
            else
            {
                gameObject.SetActive(false);
            }
        });
    }
}
