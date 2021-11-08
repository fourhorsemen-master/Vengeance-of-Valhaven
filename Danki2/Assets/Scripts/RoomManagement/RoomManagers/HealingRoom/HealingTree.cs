using UnityEngine;

public class HealingTree : Singleton<HealingTree>
{
    [SerializeField] private InteractionText interactionText = null;
    [SerializeField] private float interactionDistance = 0;

    private void Start()
    {
        interactionText.Hide();
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
            interactionText.Hide();
        }
    }
}
