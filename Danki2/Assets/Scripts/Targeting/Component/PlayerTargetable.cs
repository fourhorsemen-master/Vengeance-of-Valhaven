using System;
using UnityEngine;

public class PlayerTargetable : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy = null;

    private const float HighlightIntensity = 0.02f;

    private Guid highlightId = Guid.Empty;
    
    void Start()
    {
        enemy.PlayerTargeted.Subscribe(t => SetHighlighted(t));
    }

    private void SetHighlighted(bool highlighted)
    {
        if (highlighted)
        {
            highlightId = enemy.HightlightManager.AddIndefiniteHighlight(HighlightIntensity);
        }
        else if (highlightId != Guid.Empty)
        {
            enemy.HightlightManager.RemoveHighlight(highlightId);
            highlightId = Guid.Empty;
        }
    }
}
