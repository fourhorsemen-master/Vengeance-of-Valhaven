using System.Collections.Generic;
using UnityEngine;

public class AbilitySelectionMenu : MonoBehaviour
{
    [SerializeField] private AbilityOptionPanels abilityOptionPanels = null;
    [SerializeField] private AbilityOptionButtons abilityOptionButtons = null;

    private readonly List<Subscription> subscriptions = new List<Subscription>();
    private Subscription<AbilityReference> abilitySelectedSubscription = null;

    private AbilityReference selectedAbility = default;

    private void OnEnable()
    {
        abilitySelectedSubscription = abilityOptionPanels.AbilitySelectedSubject.Subscribe(HandleAbilitySelection);
        subscriptions.Add(
            abilityOptionPanels.AbilityDeselectedSubject.Subscribe(HandleAbilityDeselection),
            abilityOptionButtons.SkipSubject.Subscribe(HandleSkip),
            abilityOptionButtons.ConfirmSubject.Subscribe(() => HandleConfirm(selectedAbility))
        );
    }

    private void OnDisable()
    {
        subscriptions.ForEach(s => s.Unsubscribe());
        subscriptions.Clear();
        abilitySelectedSubscription.Unsubscribe();
        abilitySelectedSubscription = null;
    }

    private void HandleAbilitySelection(AbilityReference abilityReference)
    {
        selectedAbility = abilityReference;
        abilityOptionButtons.Interactable = true;
    }

    private void HandleAbilityDeselection()
    {
        selectedAbility = default;
        abilityOptionButtons.Interactable = false;
    }

    private void HandleSkip() => AbilityRoomManager.Instance.SkipAbilities();

    private void HandleConfirm(AbilityReference abilityReference) => AbilityRoomManager.Instance.SelectAbility(abilityReference);
}
