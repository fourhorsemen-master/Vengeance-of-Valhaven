using UnityEngine;
using UnityEngine.UI;

public class TooltipAbilityOrb : MonoBehaviour
{
    [SerializeField]
    private Image image = null;

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}