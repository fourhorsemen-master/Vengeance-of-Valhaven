using UnityEngine;

public class RoomTransitioner : MonoBehaviour
{
    private const float TransitionDistance = 1;

    [SerializeField] public int id = 0;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private Light pointLight = null;

    private void Start()
    {
        pointLight.color = color;
        GameplayRoomTransitionManager.Instance.CanTransitionSubject.Subscribe(HandleCanTransitionSubject);
    }

    private void Update()
    {
        if (!GameplayRoomTransitionManager.Instance.CanTransition) return;
        if (transform.DistanceFromPlayer() > TransitionDistance) return;

        int currentRoomId = PersistenceManager.Instance.SaveData.CurrentRoomId;
        RoomSaveData roomSaveData = PersistenceManager.Instance.SaveData.RoomSaveDataLookup[currentRoomId];
        int nextRoomId = roomSaveData.RoomTransitionerIdToNextRoomId[id];
        PersistenceManager.Instance.TransitionToNextRoom(nextRoomId);
    }

    private void HandleCanTransitionSubject(bool canTransition)
    {
        if (canTransition) pointLight.enabled = true;
    }
}
