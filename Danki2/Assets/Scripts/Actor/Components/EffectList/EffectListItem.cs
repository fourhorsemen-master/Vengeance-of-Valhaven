using UnityEngine;
using UnityEngine.UI;

public class EffectListItem : MonoBehaviour
{
    [SerializeField]
    private Image image = null;

    public EffectListItem Initialise(Effect effect)
    {
        image.sprite = effect.GetSprite();
        return this;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
