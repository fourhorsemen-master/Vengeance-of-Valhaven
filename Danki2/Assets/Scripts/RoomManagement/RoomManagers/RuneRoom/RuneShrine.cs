using UnityEngine;

public class RuneShrine : Singleton<RuneShrine>, IShrine
{
    [SerializeField] private InteractionText interactionText = null;
    [SerializeField] private float interactionDistance = 0;

    private bool runeSelected = false;

    private void Start()
    {
        interactionText.Hide();

        runeSelected = RuneRoomManager.Instance.RuneSelected;
        RuneRoomManager.Instance.RuneSelectedSubject.Subscribe(() =>
        {
            runeSelected = true;
            interactionText.Hide();
        });
    }

    private void Update()
    {
        if (CanInteract()) interactionText.Show();
        else interactionText.Hide();
    }

    public bool CanInteract() => GameplayStateController.Instance.GameplayState == GameplayState.Playing &&
                                 !runeSelected &&
                                 transform.DistanceFromPlayer() <= interactionDistance;
}
