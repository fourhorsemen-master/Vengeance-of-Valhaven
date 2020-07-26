using UnityEngine;
using UnityEngine.UI;

public class OrbsPanelItem : MonoBehaviour
{
    [SerializeField]
    private Image image = null;

    [SerializeField]
    private Text text = null;

    public void Initialise(OrbType orbType)
    {
        image.sprite = OrbLookup.Instance.GetSprite(orbType);
        text.text = OrbLookup.Instance.GetDisplayName(orbType);
    }
}
