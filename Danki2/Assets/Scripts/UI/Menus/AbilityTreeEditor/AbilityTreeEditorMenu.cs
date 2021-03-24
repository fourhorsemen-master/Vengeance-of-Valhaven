using System.Collections.Generic;
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

    private readonly List<Subscription<AbilityReference>> subscriptions = new List<Subscription<AbilityReference>>();

    private void OnEnable()
    {
        subscriptions.Add(ListAbilityDragStartSubject.Subscribe(ability => {
            AbilityDraggingFromList = ability;
            IsDraggingFromList = true;
        }));

        subscriptions.Add(ListAbilityDragStopSubject.Subscribe(ability => {
            AbilityDraggingFromList = ability;
            IsDraggingFromList = false;
        }));

        abilityListDisplay.Initialise();
        abilityTreeDisplay.Initialise();

        abilityListDisplay.PopulateAbilityList();
        abilityTreeDisplay.RecalculateDisplay();
    }

    private void OnDisable()
    {
        subscriptions.ForEach(s => s.Unsubscribe());
        subscriptions.Clear();
    }
}
