using UnityEngine;

public class RoomTransitioner : MonoBehaviour
{
    private const float TransitionDistance = 1;

    [SerializeField] private int id = 0;
    [SerializeField] private Light pointLight = null;

    private bool active = false;
    
    private void Start()
    {
        active = PersistenceManager.Instance.SaveData.CurrentRoomNode.ExitIdToChildLookup.ContainsKey(id);
        if (!active) return;

        GameplayRoomTransitionManager.Instance.CanTransitionSubject.Subscribe(HandleCanTransitionSubject);
    }

    private void Update()
    {
        if (!active) return;
        if (!GameplayRoomTransitionManager.Instance.CanTransition) return;
        if (transform.DistanceFromPlayer() > TransitionDistance) return;

        PersistenceManager.Instance.TransitionToNextRoom(
            PersistenceManager.Instance.SaveData.CurrentRoomNode.ExitIdToChildLookup[id]
        );
    }

    private void HandleCanTransitionSubject(bool canTransition)
    {
        if (canTransition) pointLight.enabled = true;
    }
}
