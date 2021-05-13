using UnityEngine;

public class RoomTransitioner : MonoBehaviour
{
    private const float TransitionDistance = 1;

    [SerializeField] private int id = 0;
    [SerializeField] private Light pointLight = null;
    [SerializeField] private TransitionPointer transitionPointerPrefab = null;

    private bool active = false;
    
    private void Start()
    {
        active = PersistenceManager.Instance.SaveData.CurrentRoomSaveData.RoomTransitionerIdToTransitionData.ContainsKey(id);
        if (!active) return;

        GameplayRoomTransitionManager.Instance.CanTransitionSubject.Subscribe(HandleCanTransitionSubject);
    }

    private void Update()
    {
        if (!active) return;
        if (!GameplayRoomTransitionManager.Instance.CanTransition) return;
        if (transform.DistanceFromPlayer() > TransitionDistance) return;

        int nextRoomId = PersistenceManager.Instance.SaveData.CurrentRoomSaveData.RoomTransitionerIdToTransitionData[id].NextRoomId;
        PersistenceManager.Instance.TransitionToNextRoom(nextRoomId);
    }

    private void HandleCanTransitionSubject(bool canTransition)
    {
        if (canTransition)
        {
            pointLight.enabled = true;
            TransitionPointer.Create(transitionPointerPrefab, transform.position);
        }
    }
}
