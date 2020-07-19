public class AbilityTreeEditorMenu : Singleton<AbilityTreeEditorMenu>
{
    public bool IsDraggingFromList { get; private set; }

    public AbilityReference AbilityDraggingFromList { get; private set; }

    public Subject<AbilityReference> AbilityDragFromListStartSubject { get; } = new Subject<AbilityReference>();

    public Subject<AbilityReference> AbilityDragFromListStopSubject { get; } = new Subject<AbilityReference>();

    public Node CurrentTreeNodeHover { get; set; } = null;

    private void Start()
    {
        AbilityDragFromListStartSubject.Subscribe(ability => {
            AbilityDraggingFromList = ability;
            IsDraggingFromList = true;
        });

        AbilityDragFromListStopSubject.Subscribe(ability => {
            AbilityDraggingFromList = ability;
            IsDraggingFromList = false;
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
