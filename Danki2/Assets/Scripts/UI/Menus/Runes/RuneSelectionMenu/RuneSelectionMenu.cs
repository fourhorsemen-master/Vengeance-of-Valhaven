using System.Collections.Generic;
using UnityEngine;

public class RuneSelectionMenu : MonoBehaviour
{
    [SerializeField] private ClickableRunePanel clickableRunePanelPrefab = null;
    
    [SerializeField] private RunePanel nextRunePanel = null;
    [SerializeField] private Transform currentRunePanels = null;

    private List<ClickableRunePanel> clickableRunePanels = new List<ClickableRunePanel>();
    
    public Subject SkipClickedSubject { get; } = new Subject();
    public Subject RuneSelectedSubject { get; } = new Subject();

    private void OnEnable()
    {
        RuneRoomManager.Instance.ViewRunes();
        
        nextRunePanel.Initialise(RuneRoomManager.Instance.NextRune);
    }

    private void OnDisable()
    {
    }
}
