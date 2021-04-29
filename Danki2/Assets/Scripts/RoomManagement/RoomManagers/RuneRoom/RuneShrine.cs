using UnityEngine;
using UnityEngine.UI;

public class RuneShrine : Singleton<RuneShrine>
{
    [SerializeField] private Text interactionText = null;
    [SerializeField] private float interactionDistance = 0;

    private bool runeSelected = false;

    private void Start()
    {
        HideInteractionText();

        runeSelected = RuneRoomManager.Instance.RuneSelected;
        RuneRoomManager.Instance.RuneSelectedSubject.Subscribe(() =>
        {
            runeSelected = true;
            HideInteractionText();
        });
    }

    private void Update()
    {
        if (CanInteract()) ShowInteractionText();
        else HideInteractionText();
    }

    public bool CanInteract() => GameplayStateController.Instance.GameplayState == GameplayState.Playing &&
                                 !runeSelected &&
                                 transform.DistanceFromPlayer() <= interactionDistance;
    
    private void ShowInteractionText() => interactionText.enabled = true;

    private void HideInteractionText() => interactionText.enabled = false;
}
