using UnityEngine;
using UnityEngine.UI;

public class EffectListItem : MonoBehaviour
{
    [SerializeField]
    private Image image = null;

    [SerializeField]
    private Image cooldown = null;

    private float totalDuration;

    public void Initialise(Effect effect)
    {
        image.sprite = effect.GetSprite();
        SetCooldownProportion(0);
    }

    public void Initialise(Effect effect, float totalDuration)
    {
        image.sprite = effect.GetSprite();
        this.totalDuration = totalDuration;
        SetCooldownProportion(1f);
    }

    public void SetRemainingDuration(float remainingDuration)
    {
        SetCooldownProportion(remainingDuration / totalDuration);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void SetCooldownProportion(float cooldownProportion)
    {
        cooldown.transform.localScale = new Vector3(1f, cooldownProportion, 1f);
    }
}
