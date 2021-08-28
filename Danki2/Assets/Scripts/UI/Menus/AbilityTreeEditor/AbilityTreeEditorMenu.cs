using System.Collections.Generic;
using UnityEngine;

public class AbilityTreeEditorMenu : Singleton<AbilityTreeEditorMenu>
{
    [SerializeField]
    private AbilityListDisplay abilityListDisplay = null;

    [SerializeField]
    private AbilityTreeDisplay abilityTreeDisplay = null;

    public bool IsDraggingFromList { get; private set; }

    public Ability2 AbilityDraggingFromList { get; private set; }

    public Subject<Ability2> ListAbilityDragStartSubject { get; } = new Subject<Ability2>();

    public Subject<Ability2> ListAbilityDragStopSubject { get; } = new Subject<Ability2>();

    public Subject TreeAbilityDragStopSubject { get; } = new Subject();

    public Node CurrentTreeNodeHover { get; set; } = null;

    private readonly List<Subscription<Ability2>> subscriptions = new List<Subscription<Ability2>>();

    public AbilityTooltip CreateTooltip(Ability2 ability)
    {
        return AbilityTooltip.Create(transform, ability);
    }

    public AbilityTooltip CreateTooltip(Node node)
    {
        return AbilityTooltip.Create(transform, node);
    }
    
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
