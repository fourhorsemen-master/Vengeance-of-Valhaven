using UnityEngine;
using UnityEngine.UI;

public class TooltipAbilityOrb : MonoBehaviour
{
    [SerializeField]
    private Image image = null;

    public void ShiftRight(float amount)
    {
        Vector3 newPosition = image.rectTransform.localPosition;
        newPosition += Vector3.right * amount;
        image.rectTransform.localPosition = newPosition;
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}