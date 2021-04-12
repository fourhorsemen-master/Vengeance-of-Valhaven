using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionallyDestroyed : MonoBehaviour
{
    [SerializeField] private bool hasAssociatedEntrance = false;
    [SerializeField] private int associatedEntranceId = 0;
    [SerializeField] private bool hasAssociatedExit = false;
    [SerializeField] private int associatedExitId = 0;

    public bool HasAssociatedEntrance { get => hasAssociatedEntrance; set => hasAssociatedEntrance = value; }
    public int AssociatedEntranceId { get => associatedEntranceId; set => associatedEntranceId = value; }
    public bool HasAssociatedExit { get => hasAssociatedExit; set => hasAssociatedExit = value; }
    public int AssociatedExitId { get => associatedExitId; set => associatedExitId = value; }

    private void Start()
    {
        RoomSaveData currentRoomSaveData = PersistenceManager.Instance.SaveData.CurrentRoomSaveData;

        if (hasAssociatedEntrance && associatedEntranceId == currentRoomSaveData.PlayerSpawnerId) return;

        List<int> activeExitIds = currentRoomSaveData.RoomTransitionerIdToTransitionData.Keys.ToList();
        if (hasAssociatedExit && activeExitIds.Contains(associatedExitId)) return;

        Destroy(gameObject);
    }
}
