using UnityEngine;

public class TransitionSocket : MonoBehaviour, IIdentifiable
{
    [SerializeField] private int id = 0;
    [SerializeField] private int associatedExitId = 0;
    [SerializeField] private GameObject navBlocker = null;
    [SerializeField] private GameObject directionIndicator = null;

    public int Id { get => id; set => id = value; }
    public int AssociatedExitId { get => associatedExitId; set => associatedExitId = value; }
    public GameObject NavBlocker { get => navBlocker; set => navBlocker = value; }
    public GameObject DirectionIndicator { get => directionIndicator; set => directionIndicator = value; }

    private void Start()
    {
        Destroy(NavBlocker);
        Destroy(DirectionIndicator);
    }
}
