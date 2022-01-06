using UnityEngine;

public class TransitionDependent : MonoBehaviour
{
    [SerializeField] private TransitionDependenceType type = TransitionDependenceType.ActiveWithTransition;
    [SerializeField] private bool hasAssociatedEntrance = false;
    [SerializeField] private int associatedEntranceId = 0;
    [SerializeField] private bool hasAssociatedExit = false;
    [SerializeField] private int associatedExitId = 0;

    public TransitionDependenceType Type { get => type; set => type = value; }
    public bool HasAssociatedEntrance { get => hasAssociatedEntrance; set => hasAssociatedEntrance = value; }
    public int AssociatedEntranceId { get => associatedEntranceId; set => associatedEntranceId = value; }
    public bool HasAssociatedExit { get => hasAssociatedExit; set => hasAssociatedExit = value; }
    public int AssociatedExitId { get => associatedExitId; set => associatedExitId = value; }

    private void Start()
    {
        bool transitionIsActive = TransitionIsActive();

        if (transitionIsActive && type == TransitionDependenceType.ActiveWithoutTransition) Destroy(gameObject);
        if (!transitionIsActive && type == TransitionDependenceType.ActiveWithTransition) Destroy(gameObject);
    }

    private bool TransitionIsActive()
    {
        RoomNode currentRoomNode = PersistenceManager.Instance.SaveData.CurrentRoomNode;

        if (hasAssociatedEntrance && associatedEntranceId == currentRoomNode.PlayerSpawnerId) return true;

        return hasAssociatedExit && currentRoomNode.ExitIdToChildLookup.ContainsKey(associatedExitId);
    }
}
