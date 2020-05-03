using UnityEngine;
using UnityEngine.UI;

public class TreeAbility : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private Image abilityImage = null;

    public void ShiftRight(float amount)
    {
        Vector3 newPosition = rectTransform.localPosition;
        newPosition += Vector3.right * amount;
        rectTransform.localPosition = newPosition;
    }

    public void SetImage(Sprite sprite)
    {
        abilityImage.sprite = sprite;
    }
}
