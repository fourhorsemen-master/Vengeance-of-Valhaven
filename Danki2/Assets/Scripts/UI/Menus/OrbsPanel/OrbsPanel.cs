using UnityEngine;

public class OrbsPanel : MonoBehaviour
{
    [SerializeField]
    private OrbsPanelItem orbsPanelItemPrefab = null;
    
    private void Start()
    {
        EnumUtils.ForEach<OrbType>(orbType =>
        {
            Instantiate(orbsPanelItemPrefab, Vector3.zero, Quaternion.identity, transform)
                .Initialise(orbType);
        });
    }
}
