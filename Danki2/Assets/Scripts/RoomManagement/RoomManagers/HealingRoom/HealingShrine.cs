using UnityEngine;

public class HealingShrine : Singleton<HealingShrine>
{
    [SerializeField] private InteractionText interactionText = null;
    [SerializeField] private float interactionDistance = 0;
    [SerializeField] private CollectibleOrb collectibleOrb = null;

    private void Start()
    {
        interactionText.Hide();

        if (HealingRoomManager.Instance.HasHealed)
        {
            collectibleOrb.Destroy();
        }
    }

    private void Update()
    {
        if (HealingRoomManager.Instance.HasHealed || GameplayStateController.Instance.GameplayState != GameplayState.Playing)
        {
            interactionText.Hide();
            return;
        }

        if (transform.DistanceFromPlayer() <= interactionDistance)
        {
            interactionText.Show();
            ListenForInteraction();
        }
        else
        {
            interactionText.Hide();
        }
    }

    private void ListenForInteraction()
    {
        if (Input.GetButtonDown("Interact"))
        {
            HealingRoomManager.Instance.Heal();
            collectibleOrb.Collect();
            interactionText.Hide();
        }
    }
}
