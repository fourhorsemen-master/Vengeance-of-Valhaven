using System.Collections.Generic;
using UnityEngine;

public class AbilityTreeEditorMenu : Singleton<AbilityTreeEditorMenu>
{
    [SerializeField]
    private AbilityListDisplay abilityListDisplay = null;

    [SerializeField]
    private AbilityTreeDisplay abilityTreeDisplay = null;

    public bool IsDraggingFromList { get; private set; }

    public SerializableGuid AbilityDraggingFromList { get; private set; }

    public Subject<SerializableGuid> ListAbilityDragStartSubject { get; } = new Subject<SerializableGuid>();

    public Subject<SerializableGuid> ListAbilityDragStopSubject { get; } = new Subject<SerializableGuid>();

    public Subject TreeAbilityDragStopSubject { get; } = new Subject();

    public Node CurrentTreeNodeHover { get; set; } = null;

    private readonly List<Subscription<SerializableGuid>> subscriptions = new List<Subscription<SerializableGuid>>();

    public AbilityTooltip CreateTooltip(SerializableGuid abilityId)
    {
        return AbilityTooltip.Create(transform, abilityId);
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
