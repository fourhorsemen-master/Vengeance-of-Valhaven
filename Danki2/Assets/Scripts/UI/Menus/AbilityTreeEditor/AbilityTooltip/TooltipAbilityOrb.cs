using UnityEngine;
using UnityEngine.UI;

public class TooltipAbilityOrb : MonoBehaviour
{
    [SerializeField]
    private Image image = null;

    public void SetType(OrbType orbType)
    {
        Sprite sprite = OrbLookup.Instance.GetSprite(orbType);
        image.sprite = sprite;        
    }
}