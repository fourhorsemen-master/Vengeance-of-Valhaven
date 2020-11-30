using UnityEngine;
using UnityEngine.UI;

public class EffectListItem : MonoBehaviour
{
    [SerializeField]
    private Image image = null;

    [SerializeField]
    private Image cooldown = null;

    private float totalDuration;

    private int stacks;

    public EffectListItem InitialiseActiveEffect(ActiveEffect effect, float totalDuration)
    {
        image.sprite = EffectLookup.Instance.GetSprite(effect);
        this.totalDuration = totalDuration;
        SetCooldownProportion(1);
        return this;
    }

    public EffectListItem InitialisePassiveEffect(PassiveEffect effect)
    {
        image.sprite = EffectLookup.Instance.GetSprite(effect);
        SetCooldownProportion(1);
        return this;
    }

    public EffectListItem InitialiseStackingEffect(StackingEffect effect, int stacks)
    {
        image.sprite = EffectLookup.Instance.GetSprite(effect);
        totalDuration = EffectLookup.Instance.GetStackingEffectDuration(effect);
        this.stacks = stacks;
        SetCooldownProportion(1);
        return this;
    }

    public void ResetTotalDuration(float totalDuration)
    {
        this.totalDuration = totalDuration;
    }

    public void UpdateStacks(int stacks)
    {
        this.stacks = stacks;
    }

    public void UpdateRemainingDuration(float remainingDuration)
    {
        SetCooldownProportion(remainingDuration / totalDuration);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void SetCooldownProportion(float remainingCooldownProportion)
    {
        cooldown.transform.localScale = new Vector3(1f, 1 - remainingCooldownProportion, 1f);
    }
}
