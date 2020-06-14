public class AbilityTreeEditorMenu : Singleton<AbilityTreeEditorMenu>
{
    public bool DraggingAbility = false;

    private void Start()
    {
        GameStateController.Instance.GameStateTransitionSubject.Subscribe(gameState =>
        {
            if (gameState == GameState.InAbilityTreeEditor)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        });
    }
}
