using System.Collections.Generic;
using UnityEngine;

public class AbilityOptionsPanel : MonoBehaviour
{
    [SerializeField]
    private List<AbilityOptionPanel> abilityOptionPanels = null;

    public Subject<AbilityReference> AbilitySelectedSubject { get; } = new Subject<AbilityReference>();

    public Subject AbilityDeselectedSubject { get; } = new Subject();

    private readonly List<Subscription> subscriptions = new List<Subscription>();

    private void OnEnable()
    {
        InitialisePanels(PersistenceManager.Instance.SaveData.CurrentRoomSaveData.AbilityRoomSaveData.AbilityChoices);
    }

    private void OnDisable()
    {
        subscriptions.ForEach(s => s.Unsubscribe());
        subscriptions.Clear();
    }

    private void InitialisePanels(List<AbilityReference> options)
    {
        if (options.Count != abilityOptionPanels.Count)
        {
            Debug.LogError($"{options.Count} ability options given, but only {abilityOptionPanels.Count} ability option panels");
            return;
        }

        for (int i = 0; i < options.Count; i++)
        {
            AbilityOptionPanel abilityOptionPanel = abilityOptionPanels[i];
            AbilityReference abilityReference = options[i];

            abilityOptionPanel.Initialise(abilityReference);
            subscriptions.Add(
                abilityOptionPanel.OnClickSubject.Subscribe(() => OnOptionClicked(abilityOptionPanel))
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
}
