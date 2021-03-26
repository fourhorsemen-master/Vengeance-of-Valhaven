using UnityEngine;
using UnityEngine.UI;

public class AbilityShrine : Singleton<AbilityShrine>
{
    [SerializeField] private Text interactionText = null;
    [SerializeField] private float interactionDistance = 0;

    private bool abilitySelected = false;

    private void Start()
    {
        HideInteractionText();

        abilitySelected = AbilitySelectionRoomManager.Instance.AbilitySelected;
        AbilitySelectionRoomManager.Instance.AbilitySelectedSubject.Subscribe(() =>
        {
            abilitySelected = true;
            HideInteractionText();
        });
    }

    private void Update()
    {
        if (GameplayStateController.Instance.GameplayState != GameplayState.Playing) return;
        if (abilitySelected) return;

        if (transform.DistanceFromPlayer() <= interactionDistance)
        {
            ShowInteractionText();
            ListenForInteraction();
        }
        else
        {
            HideInteractionText();
        }
    }

    private void ShowInteractionText() => interactionText.enabled = true;

    private void HideInteractionText() => interactionText.enabled = false;

    private void ListenForInteraction()
    {
        if (Input.GetButtonDown("Interact"))
        {
            AbilitySelectionRoomManager.Instance.ViewAbilities();
            HideInteractionText();
        }
    }
}
