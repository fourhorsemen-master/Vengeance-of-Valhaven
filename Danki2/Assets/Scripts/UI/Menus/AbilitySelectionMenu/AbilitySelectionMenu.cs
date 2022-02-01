using System.Collections.Generic;
using UnityEngine;

public class AbilitySelectionMenu : MonoBehaviour
{
    [SerializeField] private AbilityOptionPanels abilityOptionPanels = null;
    [SerializeField] private AbilityOptionButtons abilityOptionButtons = null;

    private readonly List<ISubscription> subscriptions = new List<ISubscription>();

    private Ability selectedAbility = default;

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

    private void HandleAbilitySelection(Ability ability)
    {
        selectedAbility = ability;
        abilityOptionButtons.CanConfirm = true;
    }

    private void HandleAbilityDeselection()
    {
        selectedAbility = null;
        abilityOptionButtons.CanConfirm = false;
    }

    private void HandleSkip() => SkipClickedSubject.Next();

    private void HandleConfirm(Ability ability)
    {
        AbilitySelectionRoomManager.Instance.SelectAbility(ability);
        ConfirmClickedSubject.Next();
    }
}
