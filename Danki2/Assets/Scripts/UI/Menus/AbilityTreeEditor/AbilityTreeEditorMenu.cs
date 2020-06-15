public class AbilityTreeEditorMenu : Singleton<AbilityTreeEditorMenu>
{
    public bool IsDragging { get; private set; }
    public AbilityReference AbilityDragging { get; private set; }

    public Subject<AbilityReference> AbilityDragStartSubject { get; } = new Subject<AbilityReference>();

    public Subject<AbilityReference> AbilityDragStopSubject { get; } = new Subject<AbilityReference>();

    private void Start()
    {
        AbilityDragStartSubject.Subscribe(ability => {
            AbilityDragging = ability;
            IsDragging = true;
        });

        AbilityDragStopSubject.Subscribe(ability => {
            AbilityDragging = ability;
            IsDragging = false;
        });

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
