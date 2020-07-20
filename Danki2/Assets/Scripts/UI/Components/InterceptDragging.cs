using UnityEngine;
using UnityEngine.EventSystems;

public class InterceptDragging : MonoBehaviour, IDragHandler
{
    // By implementing this interface, we block elemnts behind from getting dragged.
    public void OnDrag(PointerEventData eventData) { }
}
