using UnityEngine;

public class AbilityShrine : Singleton<AbilityShrine>, IShrine
{
    [SerializeField] private InteractionText interactionText = null;
    [SerializeField] private float interactionDistance = 0;
    [SerializeField] private CollectibleOrb collectibleOrb = null;

    private bool abilitySelected = false;

    private void Start()
    {
        interactionText.Hide();

        abilitySelected = AbilitySelectionRoomManager.Instance.AbilitySelected;
        AbilitySelectionRoomManager.Instance.AbilitySelectedSubject.Subscribe(() =>
        {
            abilitySelected = true;
            collectibleOrb.Collect();
            interactionText.Hide();
        });
    }

    private void Update()
    {
        if (CanInteract()) interactionText.Show();
        else interactionText.Hide();
    }

    public bool CanInteract() => GameplayStateController.Instance.GameplayState == GameplayState.Playing &&
                                 !abilitySelected &&
                                 transform.DistanceFromPlayer() <= interactionDistance;
}
