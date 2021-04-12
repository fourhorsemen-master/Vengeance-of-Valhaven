using System.Collections.Generic;
using UnityEngine;

public class ModuleSocket : MonoBehaviour, IIdentifiable
{
    [SerializeField] private int id = 0;
    [SerializeField] private GameObject navBlocker = null;
    [SerializeField] private GameObject directionIndicator = null;
    [SerializeField] private SocketType socketType = default;
    [SerializeField] private List<ModuleTag> tags = new List<ModuleTag>();
    [SerializeField] private List<ModuleTag> tagsToExclude = new List<ModuleTag> {ModuleTag.Floating};

    public int Id { get => id; set => id = value; }
    public GameObject NavBlocker { get => navBlocker; set => navBlocker = value; }
    public GameObject DirectionIndicator { get => directionIndicator; set => directionIndicator = value; }
    public SocketType SocketType { get => socketType; set => socketType = value; }
    public List<ModuleTag> Tags { get => tags; set => tags = value; }
    public List<ModuleTag> TagsToExclude { get => tagsToExclude; set => tagsToExclude = value; }

    private void Start()
    {
        if (navBlocker) Destroy(navBlocker);
        if (directionIndicator) Destroy(directionIndicator);
    }
}
