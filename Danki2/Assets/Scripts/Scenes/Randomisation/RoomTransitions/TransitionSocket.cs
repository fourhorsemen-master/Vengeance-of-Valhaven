using System.Collections.Generic;
using UnityEngine;

public class TransitionSocket : MonoBehaviour, IIdentifiable
{
    [SerializeField] private int id = 0;
    [SerializeField] private bool hasAssociatedEntrance = false;
    [SerializeField] private int associatedEntranceId = 0;
    [SerializeField] private bool hasAssociatedExit = false;
    [SerializeField] private int associatedExitId = 0;
    [SerializeField] private List<ModuleTag> blockerTags = new List<ModuleTag>();
    [SerializeField] private List<ModuleTag> exitTags = new List<ModuleTag>();

    [SerializeField] private GameObject navBlocker = null;
    [SerializeField] private GameObject directionIndicator = null;
    
    public int Id { get => id; set => id = value; }
    public bool HasAssociatedEntrance { get => hasAssociatedEntrance; set => hasAssociatedEntrance = value; }
    public int AssociatedEntranceId { get => associatedEntranceId; set => associatedEntranceId = value; }
    public bool HasAssociatedExit { get => hasAssociatedExit; set => hasAssociatedExit = value; }
    public int AssociatedExitId { get => associatedExitId; set => associatedExitId = value; }
    public List<ModuleTag> BlockerTags { get => blockerTags; set => blockerTags = value; }
    public List<ModuleTag> ExitTags { get => exitTags; set => exitTags = value; }

    public GameObject NavBlocker { get => navBlocker; set => navBlocker = value; }
    public GameObject DirectionIndicator { get => directionIndicator; set => directionIndicator = value; }

    private void Start()
    {
        Destroy(NavBlocker);
        Destroy(DirectionIndicator);
    }
}
