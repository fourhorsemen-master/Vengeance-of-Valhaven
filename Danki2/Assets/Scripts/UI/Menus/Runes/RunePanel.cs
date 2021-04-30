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
    private RuneTooltip runeTooltip;
    
    public static RunePanel Create(RunePanel prefab, Transform transform, Rune? rune = null)
    {
        RunePanel runePanel = Instantiate(prefab, transform);
        runePanel.Initialise(rune);
        return runePanel;
    }

    private void OnDestroy() => TryDestroyTooltip();

    public void PointerEnter()
    {
        if (rune.HasValue) runeTooltip = RuneTooltip.Create(rune.Value, transform.parent);
    }

    public void PointerExit() => TryDestroyTooltip();
    
    public void Initialise(Rune? rune = null)
    {
        this.rune = rune;

        if (this.rune.HasValue)
        {
            frame.sprite = filledBorder;
            image.sprite = RuneLookup.Instance.GetSprite(this.rune.Value);
            text.text = RuneLookup.Instance.GetDisplayName(this.rune.Value);
            return;
        }

        frame.sprite = emptyBorder;
        Destroy(image.gameObject);
        Destroy(text.gameObject);
    }

    private void TryDestroyTooltip()
    {
        if (runeTooltip) runeTooltip.Destroy();
    }
}
