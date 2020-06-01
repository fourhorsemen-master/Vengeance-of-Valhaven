using UnityEngine;

public class PlayerTargetable : MonoBehaviour
{
    public static readonly Color HighlightedColor = new Color(0.02f, 0.02f, 0.02f);

    [SerializeField]
    private MonoBehaviour targetableMonobehaviour = null;

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    private ITargetable targetable = null;

    private void Awake()
    {
        if (targetableMonobehaviour is ITargetable)
        {
            targetable = targetableMonobehaviour as ITargetable;
        }

        Debug.LogError($"Non targetable of type '{targetable.GetType()}' passed in as targetable.");
    }

    void Start()
    {
        targetable.PlayerTargeted.Subscribe(t => SetHighlighted(t));
    }

    private void SetHighlighted(bool highlighted)
    {
        Color colour = highlighted ? HighlightedColor : Color.clear;

        meshRenderer.material.SetEmissiveColour(colour);
    }
}
