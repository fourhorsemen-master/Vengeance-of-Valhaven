using UnityEngine;

public class OrbsPanel : MonoBehaviour
{
    [SerializeField]
    private OrbsPanelItem orbsPanelItemPrefab = null;
    
    private void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        EnumUtils.ForEach<OrbType>(orbType =>
            {
                Debug.Log($"Creating {orbType}");
                Instantiate(orbsPanelItemPrefab, Vector3.zero, Quaternion.identity, transform);
            }
        );
    }
}
