using UnityEngine;
using UnityEngine.UI;

public class RunePanel : MonoBehaviour
{
    [SerializeField] private Sprite filledBorder = null;
    [SerializeField] private Sprite emptyBorder = null;
    [SerializeField] private Image image = null;
    [SerializeField] private Image frame = null;
    [SerializeField] private Text text = null;

    private Rune? rune;
    private Transform containerTransform;
    private RuneTooltip runeTooltip;
    
    public static RunePanel Create(RunePanel prefab, Transform transform, Rune? rune = null)
    {
        RunePanel runePanel = Instantiate(prefab, transform);
        runePanel.Initialise(transform, rune);
        return runePanel;
    }

    private void OnDestroy() => TryDestroyTooltip();

    public void PointerEnter()
    {
        if (rune.HasValue) runeTooltip = RuneTooltip.Create(rune.Value, containerTransform);
    }

    public void PointerExit() => TryDestroyTooltip();
    
    public void Initialise(Transform containerTransform, Rune? rune = null)
    {
        this.rune = rune;
        this.containerTransform = containerTransform;

        if (this.rune.HasValue)
        {
            frame.sprite = filledBorder;
            image.sprite = RuneLookup.Instance.GetSprite(this.rune.Value);
            text.text = RuneLookup.Instance.GetDisplayName(this.rune.Value);
        }
        else
        {
            frame.sprite = emptyBorder;
            Destroy(image.gameObject);
            Destroy(text.gameObject);
        }
    }

    private void TryDestroyTooltip()
    {
        if (runeTooltip) runeTooltip.Destroy();
    }
}
