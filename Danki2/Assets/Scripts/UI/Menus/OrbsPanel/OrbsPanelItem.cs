using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrbsPanelItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image image = null;

    [SerializeField]
    private Text text = null;

    private OrbType orbType;

    public void Initialise(OrbType orbType)
    {
        this.orbType = orbType;
        
        image.sprite = OrbLookup.Instance.GetSprite(orbType);
        text.text = OrbLookup.Instance.GetDisplayName(orbType);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OrbTooltip.Instance.Activate(orbType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OrbTooltip.Instance.Deactivate();
    }
}
