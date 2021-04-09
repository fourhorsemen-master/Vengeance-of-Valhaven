using UnityEngine;
using UnityEngine.UI;

public class HealingTree : Singleton<HealingTree>
{
    [SerializeField] private Text interactionText = null;
    [SerializeField] private float interactionDistance = 0;

    private void Start()
    {
        HideInteractionText();
    }

    private void Update()
    {
        if (HealingRoomManager.Instance.HasHealed || GameplayStateController.Instance.GameplayState != GameplayState.Playing)
        {
            HideInteractionText();
            return;
        }

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
            HealingRoomManager.Instance.Heal();
            HideInteractionText();
        }
    }
}
