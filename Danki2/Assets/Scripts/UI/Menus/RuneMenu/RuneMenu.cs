using System.Collections.Generic;
using UnityEngine;

public class RuneMenu : MonoBehaviour
{
    [SerializeField] private Transform runePanelParent = null;
    [SerializeField] private RunePanel runePanelPrefab = null;

    private readonly List<RunePanel> runePanels = new List<RunePanel>();
    
    private void OnEnable()
    {
        List<Rune> runes = ActorCache.Instance.Player.RuneManager.Runes;
        runes.ForEach(CreateRunePanel);
    }

    private void CreateRunePanel(Rune rune)
    {
        RunePanel runePanel = RunePanel.Create(rune, runePanelPrefab, runePanelParent);
        runePanels.Add(runePanel);
    }

    private void OnDisable()
    {
        runePanels.ForEach(p => p.Destroy());
        runePanels.Clear();
    }
}
