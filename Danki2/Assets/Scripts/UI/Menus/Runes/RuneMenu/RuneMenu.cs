using System.Collections.Generic;
using UnityEngine;

public class RuneMenu : MonoBehaviour
{
    [SerializeField] private Transform runePanelsParent = null;
    [SerializeField] private RunePanel runePanelPrefab = null;

    private readonly List<RunePanel> runePanels = new List<RunePanel>();
    
    private void OnEnable()
    {
        List<RuneSocket> runeSockets = ActorCache.Instance.Player.RuneManager.RuneSockets;
        runeSockets.ForEach(CreateRunePanel);
    }

    private void CreateRunePanel(RuneSocket runeSocket)
    {
        RunePanel runePanel = RunePanel.Create(
            runePanelPrefab,
            runePanelsParent,
            runeSocket.HasRune ? runeSocket.Rune : (Rune?) null
        );
        runePanels.Add(runePanel);
    }
    
    private void OnDisable()
    {
        runePanels.ForEach(p => Destroy(p.gameObject));
        runePanels.Clear();
    }
}
