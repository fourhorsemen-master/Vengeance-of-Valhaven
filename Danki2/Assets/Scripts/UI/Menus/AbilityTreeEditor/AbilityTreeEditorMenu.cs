using UnityEngine;

public class AbilityTreeEditorMenu : Singleton<AbilityTreeEditorMenu>
{
    [SerializeField]
    private AbilityListDisplay abilityListDisplay = null;

    [SerializeField]
    private AbilityTreeDisplay abilityTreeDisplay = null;

    public bool IsDraggingFromList { get; private set; }

    public AbilityReference AbilityDraggingFromList { get; private set; }

    public Subject<AbilityReference> ListAbilityDragStartSubject { get; } = new Subject<AbilityReference>();

    public Subject<AbilityReference> ListAbilityDragStopSubject { get; } = new Subject<AbilityReference>();

    public Subject TreeAbilityDragStopSubject { get; } = new Subject();

    public Node CurrentTreeNodeHover { get; set; } = null;

    private void Start()
    {
        ListAbilityDragStartSubject.Subscribe(ability => {
            AbilityDraggingFromList = ability;
            IsDraggingFromList = true;
        });

        ListAbilityDragStopSubject.Subscribe(ability => {
            AbilityDraggingFromList = ability;
            IsDraggingFromList = false;
        });

        abilityListDisplay.Initialise();
        abilityTreeDisplay.Initialise();

        GameStateController.Instance.GameStateTransitionSubject.Subscribe(gameState =>
        {
            if (gameState == GameState.InAbilityTreeEditor)
            {
                gameObject.SetActive(true);
                abilityListDisplay.PopulateAbilityList();
                abilityTreeDisplay.RecalculateDisplay();
            }
            else
            {
                gameObject.SetActive(false);
            }
        });
    }
}
