using System.Collections.Generic;
using UnityEngine;

public class AbilityTreeEditorMenu : Singleton<AbilityTreeEditorMenu>
{
    [SerializeField]
    private AbilityListDisplay abilityListDisplay = null;

    [SerializeField]
    private AbilityTreeDisplay abilityTreeDisplay = null;

    public bool IsDraggingFromList { get; private set; }

    public Ability AbilityDraggingFromList { get; private set; }

    public Subject<Ability> ListAbilityDragStartSubject { get; } = new Subject<Ability>();

    public Subject<Ability> ListAbilityDragStopSubject { get; } = new Subject<Ability>();

    public Subject TreeAbilityDragStopSubject { get; } = new Subject();

    public Node CurrentTreeNodeHover { get; set; } = null;

    private readonly List<Subscription<Ability>> subscriptions = new List<Subscription<Ability>>();

    public AbilityTooltip CreateTooltip(Ability ability)
    {
        return AbilityTooltip.Create(transform, ability);
    }

    public AbilityTooltip CreateTooltip(Node node)
    {
        return AbilityTooltip.Create(transform, node);
    }
    
    private void OnEnable()
    {
        subscriptions.Add(ListAbilityDragStartSubject.Subscribe(abilityId => {
            AbilityDraggingFromList = abilityId;
            IsDraggingFromList = true;
        }));

        subscriptions.Add(ListAbilityDragStopSubject.Subscribe(abilityId => {
            AbilityDraggingFromList = abilityId;
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
