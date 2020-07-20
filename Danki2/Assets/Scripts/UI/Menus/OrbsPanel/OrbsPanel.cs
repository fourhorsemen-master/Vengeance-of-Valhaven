using UnityEngine;

public class OrbsPanel : MonoBehaviour
{
    private void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        EnumUtils.ForEach<OrbType>(orbType =>
        {
            Debug.Log(OrbLookup.Instance.GetDisplayName(orbType));
        });
    }
}
