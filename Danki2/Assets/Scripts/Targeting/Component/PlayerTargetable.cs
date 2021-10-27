using UnityEngine;

public class PlayerTargetable : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy = null;
    
    void Start()
    {
        enemy.PlayerTargeted.Subscribe(t => SetHighlighted(t));
    }

    private void SetHighlighted(bool highlighted)
    {
        if (highlighted)
        {
            enemy.EmissiveManager.StartHighlight();
        }
        else
        {
            enemy.EmissiveManager.StopHighlight();
        }
    }
}
