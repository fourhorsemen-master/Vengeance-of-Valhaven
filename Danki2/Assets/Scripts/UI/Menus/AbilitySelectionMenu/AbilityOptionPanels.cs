using System.Collections.Generic;
using UnityEngine;

public class AbilityOptionPanels : MonoBehaviour
{
    [SerializeField]
    private AbilityOptionPanel abilityOptionPanelPrefab = null;

    private readonly List<AbilityOptionPanel> abilityOptionPanels = new List<AbilityOptionPanel>();
    private readonly List<Subscription> subscriptions = new List<Subscription>();

    private AbilityTooltip abilityTooltip;

    public Subject<Ability2> AbilitySelectedSubject { get; } = new Subject<Ability2>();
    public Subject AbilityDeselectedSubject { get; } = new Subject();

    private void OnEnable()
    {
        InitialisePanels(PersistenceManager.Instance.SaveData.CurrentRoomNode.AbilityRoomSaveData.AbilityChoices);
    }

    private void OnDisable()
    {
        abilityOptionPanels.ForEach(p => Destroy(p.gameObject));
        abilityOptionPanels.Clear();
        subscriptions.ForEach(s => s.Unsubscribe());
        subscriptions.Clear();
        TryDestroyTooltip();
    }

    private void InitialisePanels(List<Ability2> options)
    {
        options.ForEach(option =>
        {
            AbilityOptionPanel abilityOptionPanel = Instantiate(abilityOptionPanelPrefab, transform);
            abilityOptionPanels.Add(abilityOptionPanel);
            abilityOptionPanel.Initialise(option);
            subscriptions.Add(
                abilityOptionPanel.OnClickSubject.Subscribe(() => OnOptionClicked(abilityOptionPanel)),
                abilityOptionPanel.OnPointerEnterSubject.Subscribe(() => OnPointerEnter(abilityOptionPanel)),
                abilityOptionPanel.OnPointerExitSubject.Subscribe(() => OnPointerExit(abilityOptionPanel))
            );
        });
    }

    private void OnOptionClicked(AbilityOptionPanel abilityOptionPanel)
    {
        abilityOptionPanels.ForEach(panel =>
        {
            if (panel == abilityOptionPanel) panel.Selected = !panel.Selected;
            else panel.Selected = false;
        });

        if (abilityOptionPanel.Selected) AbilitySelectedSubject.Next(abilityOptionPanel.Ability);
        else AbilityDeselectedSubject.Next();
    }

    private void OnPointerEnter(AbilityOptionPanel abilityOptionPanel)
    {
        TryDestroyTooltip();
        abilityTooltip = AbilityTooltip.Create(transform, abilityOptionPanel.Ability);
        abilityOptionPanel.Highlighted = true;
    }

    private void OnPointerExit(AbilityOptionPanel abilityOptionPanel)
    {
        TryDestroyTooltip();
        abilityOptionPanel.Highlighted = false;
    }

    private void TryDestroyTooltip()
    {
        if (abilityTooltip) abilityTooltip.Destroy();
    }
}
