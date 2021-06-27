using UnityEngine;
using UnityEngine.UI;

public class InteractionText : MonoBehaviour
{
    [SerializeField]
    private Text interactionText = null;

    public void Show()
    {
        interactionText.enabled = true;
    }

    public void Hide()
    {
        interactionText.enabled = false;
    }
}
