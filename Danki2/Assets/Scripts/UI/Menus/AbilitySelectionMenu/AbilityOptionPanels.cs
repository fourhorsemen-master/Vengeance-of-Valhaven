using System.Collections.Generic;
using UnityEngine;

public class AbilityOptionPanels : MonoBehaviour
{
    [SerializeField]
    private List<AbilityOptionPanel> abilityOptionPanels = null;

    public Subject<AbilityReference> AbilitySelectedSubject { get; } = new Subject<AbilityReference>();

    public Subject AbilityDeselectedSubject { get; } = new Subject();

    private readonly List<Subscription> subscriptions = new List<Subscription>();

    private AbilityTooltip abilityTooltip;

    private void OnEnable()
    {
        InitialisePanels(PersistenceManager.Instance.SaveData.CurrentRoomSaveData.AbilityRoomSaveData.AbilityChoices);
    }

    private void OnDisable()
    {
        subscriptions.ForEach(s => s.Unsubscribe());
        subscriptions.Clear();
        TryDestroyTooltip();
    }

    private void InitialisePanels(List<AbilityReference> options)
    {
        if (options.Count != abilityOptionPanels.Count)
        {
            Debug.LogError(
                "Ability option count must be the same as the ability option panel count. " +
                $"Received {options.Count} options, but there are {abilityOptionPanels.Count} panels."
            );
            return;
        }

        for (int i = 0; i < options.Count; i++)
        {
            AbilityOptionPanel abilityOptionPanel = abilityOptionPanels[i];
            abilityOptionPanel.Initialise(options[i]);
            subscriptions.Add(
                abilityOptionPanel.OnClickSubject.Subscribe(() => OnOptionClicked(abilityOptionPanel)),
                abilityOptionPanel.OnPointerEnterSubject.Subscribe(() => OnPointerEnter(abilityOptionPanel)),
                abilityOptionPanel.OnPointerExitSubject.Subscribe(() => OnPointerExit(abilityOptionPanel))
            );
        }
    }

    private void OnOptionClicked(AbilityOptionPanel abilityOptionPanel)
    {
        abilityOptionPanels.ForEach(panel =>
        {
            if (panel == abilityOptionPanel) panel.Selected = !panel.Selected;
            else panel.Selected = false;
        });

        if (abilityOptionPanel.Selected) AbilitySelectedSubject.Next(abilityOptionPanel.AbilityReference);
        else AbilityDeselectedSubject.Next();
    }

    private void OnPointerEnter(AbilityOptionPanel abilityOptionPanel)
    {
        TryDestroyTooltip();
        abilityTooltip = AbilityTooltip.Create(transform, abilityOptionPanel.AbilityReference);
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
