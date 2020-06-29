using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityInsertionArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image icon = null;

    public Subject MouseUpSubject = new Subject();

    public void OnDrop(PointerEventData _)
    {
        MouseUpSubject.Next();
    }

    public void OnDisable()
    {
        icon.SetOpacity(0.4f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        icon.SetOpacity(1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        icon.SetOpacity(0.4f);
    }
}
