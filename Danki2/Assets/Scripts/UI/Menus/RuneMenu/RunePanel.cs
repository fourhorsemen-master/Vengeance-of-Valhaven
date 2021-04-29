using UnityEngine;
using UnityEngine.UI;

public class RunePanel : MonoBehaviour
{
    [SerializeField] private Image image = null;
    [SerializeField] private Text text = null;

    private Rune rune;
    private RuneTooltip runeTooltip;
    
    public static RunePanel Create(Rune rune, RunePanel prefab, Transform transform)
    {
        RunePanel runePanel = Instantiate(prefab, transform);
        runePanel.Initialise(rune);
        return runePanel;
    }

    private void OnDestroy() => TryDestroyTooltip();

    public void PointerEnter() => runeTooltip = RuneTooltip.Create(rune, transform.parent);

    public void PointerExit() => TryDestroyTooltip();
    
    private void Initialise(Rune rune)
    {
        image.sprite = RuneLookup.Instance.GetSprite(rune);
        text.text = RuneLookup.Instance.GetDisplayName(rune);
        this.rune = rune;
    }

    private void TryDestroyTooltip()
    {
        if (runeTooltip) runeTooltip.Destroy();
    }
}
