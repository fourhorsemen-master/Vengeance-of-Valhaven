using System.Collections.Generic;
using UnityEngine;

public class AbilitySelectionMenu : MonoBehaviour
{
    [SerializeField] private AbilityOptionPanels abilityOptionPanels = null;
    [SerializeField] private AbilityOptionButtons abilityOptionButtons = null;

    private readonly List<ISubscription> subscriptions = new List<ISubscription>();

    private AbilityReference selectedAbility = default;

    public Subject SkipClickedSubject { get; } = new Subject();
    public Subject ConfirmClickedSubject { get; } = new Subject();

    private void OnEnable()
    {
        AbilitySelectionRoomManager.Instance.ViewAbilities();

        subscriptions.Add(
            abilityOptionPanels.AbilitySelectedSubject.Subscribe(HandleAbilitySelection),
            abilityOptionPanels.AbilityDeselectedSubject.Subscribe(HandleAbilityDeselection),
            abilityOptionButtons.SkipSubject.Subscribe(HandleSkip),
            abilityOptionButtons.ConfirmSubject.Subscribe(() => HandleConfirm(selectedAbility))
        );
    }

    private void OnDisable()
    {
        subscriptions.ForEach(s => s.Unsubscribe());
        subscriptions.Clear();
    }

    private void HandleAbilitySelection(AbilityReference abilityReference)
    {
        selectedAbility = abilityReference;
        abilityOptionButtons.CanConfirm = true;
    }

    private void HandleAbilityDeselection()
    {
        selectedAbility = default;
        abilityOptionButtons.CanConfirm = false;
    }

    private void HandleSkip() => SkipClickedSubject.Next();

    private void HandleConfirm(AbilityReference abilityReference)
    {
        AbilitySelectionRoomManager.Instance.SelectAbility(abilityReference);
        ConfirmClickedSubject.Next();
    }
}
