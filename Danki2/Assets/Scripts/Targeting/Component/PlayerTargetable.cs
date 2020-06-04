using UnityEngine;

public class PlayerTargetable : MonoBehaviour
{
    public static readonly Color HighlightedColor = new Color(0.02f, 0.02f, 0.02f);

    [SerializeField]
    private Enemy enemy = null;

    [SerializeField]
    private MeshRenderer meshRenderer = null;
    
    void Start()
    {
        enemy.PlayerTargeted.Subscribe(t => SetHighlighted(t));
    }

    private void SetHighlighted(bool highlighted)
    {
        Color colour = highlighted ? HighlightedColor : Color.clear;

        meshRenderer.material.SetEmissiveColour(colour);
    }
}
