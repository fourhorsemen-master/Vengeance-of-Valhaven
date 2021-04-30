using UnityEngine;
using UnityEngine.UI;

public class ClickableRunePanel : MonoBehaviour
{
    [SerializeField] private Image highlightedIndicator = null;
    [SerializeField] private RunePanel runePanel = null;
    
    public Subject OnClickSubject { get; } = new Subject();

    public static ClickableRunePanel Create(ClickableRunePanel prefab, Transform transform, RuneSocket runeSocket)
    {
        ClickableRunePanel clickableRunePanel = Instantiate(prefab, transform);
        clickableRunePanel.Initialise(runeSocket);
        return clickableRunePanel;
    }

    private void Start()
    {
        highlightedIndicator.enabled = false;
    }

    public void Click() => OnClickSubject.Next();

    public void PointerEnter() => highlightedIndicator.enabled = true;

    public void PointerExit() => highlightedIndicator.enabled = false;

    private void Initialise(RuneSocket runeSocket)
    {
        runePanel.Initialise(runeSocket.HasRune ? runeSocket.Rune : (Rune?) null);
    }
}
