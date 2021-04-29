using System.Collections.Generic;
using UnityEngine;

public class RuneMenu : MonoBehaviour
{
    [SerializeField] private Transform runePanelsParent = null;
    [SerializeField] private RunePanel runePanelPrefab = null;
    [SerializeField] private GameObject emptyRunePanelPrefab = null;

    private readonly List<RunePanel> runePanels = new List<RunePanel>();
    private readonly List<GameObject> emptyRunePanels = new List<GameObject>();
    
    private void OnEnable()
    {
        List<Rune> runes = ActorCache.Instance.Player.RuneManager.Runes;
        runes.ForEach(CreateRunePanel);
        CreateEmptyRunePanels(RuneManager.MaxNumberOfRunes - runes.Count);
    }

    private void CreateRunePanel(Rune rune)
    {
        RunePanel runePanel = RunePanel.Create(rune, runePanelPrefab, runePanelsParent);
        runePanels.Add(runePanel);
    }

    private void CreateEmptyRunePanels(int count)
    {
        Utils.Repeat(count, () =>
        {
            GameObject emptyRunePanel = Instantiate(emptyRunePanelPrefab, runePanelsParent);
            emptyRunePanels.Add(emptyRunePanel);
        });
    }

    private void OnDisable()
    {
        runePanels.ForEach(p => Destroy(p.gameObject));
        runePanels.Clear();
        
        emptyRunePanels.ForEach(p => Destroy(p.gameObject));
        emptyRunePanels.Clear();
    }
}
