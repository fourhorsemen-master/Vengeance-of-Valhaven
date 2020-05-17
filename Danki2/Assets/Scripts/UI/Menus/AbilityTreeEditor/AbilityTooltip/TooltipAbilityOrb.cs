using UnityEngine;
using UnityEngine.UI;

public class TooltipAbilityOrb : MonoBehaviour
{
    [SerializeField]
    private Image image = null;

    [SerializeField]
    private Sprite balanceSprite = null;

    [SerializeField]
    private Sprite aggressionSprite = null;

    [SerializeField]
    private Sprite cunningSprite = null;

    public void SetType(OrbType type)
    {
        switch (type)
        {
            case OrbType.Balance:
                image.sprite = balanceSprite;
                break;

            case OrbType.Aggression:
                image.sprite = aggressionSprite;
                break;

            case OrbType.Cunning:
                image.sprite = cunningSprite;
                break;
        }
        
    }
}