public class AbilityTreeEditorMenu : Singleton<AbilityTreeEditorMenu>
{
    public bool DraggingAbility { get; private set; }

    public Subject AbilityDragStartSubject { get; } = new Subject();

    public Subject AbilityDragStopSubject { get; } = new Subject();

    private void Start()
    {
        AbilityDragStartSubject.Subscribe(() => DraggingAbility = true);
        AbilityDragStopSubject.Subscribe(() => DraggingAbility = false);

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
